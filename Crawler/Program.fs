open Crawler
open System
open System.Net.Http

[<EntryPoint>]
let main argv =
    let testUrl = "https://math.spbu.ru/rus/"
    use client = new HttpClient()
    let crawlerResult = Crawler.analyze client testUrl |> Async.RunSynchronously

    Crawler.printResults testUrl crawlerResult
    printfn "\nНажмите любую клавишу для выхода..."
    Console.ReadKey() |> ignore
    0
