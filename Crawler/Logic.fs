module Crawler

open System.Text.RegularExpressions
open System.Net.Http

/// <summary>
/// Asynchronously downloads the HTML content from the specified URL.
/// </summary>
/// <param name="url">The target URL of the web page to download.</param>
/// <returns>
/// Some containing the HTML string if successful, 
/// or None if a network error occurs.
/// </returns>
let getHtml (client: HttpClient ) (url: string) =
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
/// A list of unique URL strings (strictly starting with https:// or http://) 
/// found inside the href attributes of <a> tags.
/// </returns>
let findChildAddress htmlText =
    let pattern = @"<a\s+href\s*=\s*""(https?://[^""]+)"""
    let regex = Regex(pattern, RegexOptions.IgnoreCase)

    regex.Matches(htmlText)
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Groups.[1].Value)
    |> Seq.distinct
    |> Seq.toList

/// <summary>
/// The main coordinator function. Downloads the start page, extracts child links, 
/// and then asynchronously downloads all child pages in parallel.
/// </summary>
/// <param name="startUrl">The address of the initial page to start crawling from.</param>
/// <returns>
/// An Async workflow that resolves to an Option. 
/// If successful, returns Some containing a tuple: (StartPageSize, List of (ChildUrl, ChildSizeOption)).
/// If the start page fails to download, returns None.
/// </returns>
let analyze (client: HttpClient) startUrl =
    async {
        let! startPage = getHtml client startUrl

        match startPage with
        | None -> return None
        | Some html ->
            let startSize = html.Length
            let links = findChildAddress html

            let tasks =
                links
                |> Seq.map (fun url ->
                    async {
                        let! htmlOption = getHtml client url
                        let childSize = 
                            match htmlOption with 
                            | Some c -> Some c.Length
                            | _ -> None
                        return (url, childSize)
                    })

            let! results = Async.Parallel tasks

            return Some (startSize, Array.toList results)
    }

/// <summary>
/// Presents the crawling results by printing them to the console in a human-readable format.
/// </summary>
/// <param name="start">The URL of the start page.</param>
/// <param name="data">The structured data returned by the analyze function.</param>
let printResults start data=
    match data with 
    | None -> printfn "Не удалось скачать стартовую страницу %s" start
    | Some (startSize, childResults) ->
        printfn "Стартовая страница"
        printfn "%s - %d" start startSize
        printfn "\nДочерние страницы:"

        for (url, sizeOption) in childResults do  
            match sizeOption with
            | Some size -> printfn "%s - %d" url size
            | None -> printfn "%s - ошибка скачивания" url