module Crawler

open System.Text.RegularExpressions
open System.Net.Http

let client = new HttpClient()

let getHtml (url: string) =
    async {
        try
            let! html = client.GetStringAsync(url) |> Async.AwaitTask
            return Some html
        with
        | _ -> return None
    }

let findChildAddress (htmlText: string) =
    let pattern = @"<a\s+href\s*=\s*""(https://[^""]+)"""
    let regex = Regex(pattern, RegexOptions.IgnoreCase)

    regex.Matches(htmlText)
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Groups.[1].Value)
    |> Seq.distinct
    |> Seq.toList

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
