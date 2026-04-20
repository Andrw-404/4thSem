module Crawler

open System.Text.RegularExpressions
open System.Net.Http

/// <summary>
/// A global HTTP client instance. Created once to reuse network connections and prevent socket exhaustion.
/// </summary>
let client = new HttpClient()

/// <summary>
/// Asynchronously downloads the HTML content from the specified URL.
/// </summary>
/// <param name="url">The target URL of the web page to download.</param>
/// <returns>
/// Some containing the HTML string if successful, 
/// or None if a network error occurs.
/// </returns>
let getHtml (url: string) =
    async {
        try
            let! html = client.GetStringAsync(url) |> Async.AwaitTask
            return Some html
        with
        | _ -> return None
    }

/// <summary>
/// Extracts all unique child URLs from the provided HTML text.
/// </summary>
/// <param name="htmlText">HTML text of the web page.</param>
/// <returns>
/// A list of unique URL strings (strictly starting with https://) 
/// found inside the href attributes of <a> tags.
/// </returns>
let findChildAddress (htmlText: string) =
    let pattern = @"<a\s+href\s*=\s*""(https://[^""]+)"""
    let regex = Regex(pattern, RegexOptions.IgnoreCase)

    regex.Matches(htmlText)
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Groups.[1].Value)
    |> Seq.distinct
    |> Seq.toList

/// <summary>
/// The main coordinator function. Downloads the start page, extracts child links, 
/// and then asynchronously downloads all child pages in parallel, 
/// printing their sizes to the console.
/// </summary>
/// <param name="startUrl">The address of the initial page to start crawling from.</param>
/// <returns>An Async workflow representing the execution of the crawling process.
/// </returns>
let analyze (startUrl: string) =
    async {
        let! startPage = getHtml startUrl

        match startPage with
        | None -> printfn "Не удалось скачать стартовую страницу"
        | Some html ->
            printfn "Стартовая страница"
            printfn "%s - %d" startUrl html.Length
            let links = findChildAddress html

            let tasks =
                links
                |> Seq.map (fun url ->
                    async {
                        let! htmlOption = getHtml url
                        return (url, htmlOption)
                    })

            let! results = Async.Parallel tasks

            for (url, contentOption) in results do
                match contentOption with
                | Some contentText -> printfn "%s - %d" url contentText.Length
                | None -> printfn "%s - Ошибка скачивания" url
    }
