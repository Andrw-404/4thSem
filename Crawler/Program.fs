open Crawler
open System

[<EntryPoint>]
let main argv =
    let testUrl = "https://math.spbu.ru/rus/"

    Crawler.analyze testUrl |> Async.RunSynchronously

    printfn "\nНажмите любую клавишу для выхода..."
    Console.ReadKey() |> ignore
    0
