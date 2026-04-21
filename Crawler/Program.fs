open Crawler
open System

[<EntryPoint>]
let main argv =
    let testUrl = "https://math.spbu.ru/rus/"

    let crawlerResult = Crawler.analyze testUrl |> Async.RunSynchronously

    Crawler.printResults testUrl crawlerResult
    printfn "\nНажмите любую клавишу для выхода..."
    Console.ReadKey() |> ignore
    0
