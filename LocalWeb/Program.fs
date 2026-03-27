open Computer
open Network
open System

[<EntryPoint>]
let main argv =
    let computers =
        [ { Id = "PC1"
            OS = Windows
            IsInfected = false}
          { Id = "PC2"
            OS = MacOS
            IsInfected = true }
          { Id = "PC3"
            OS = Linux
            IsInfected = false }
          { Id = "PC4"
            OS = Windows
            IsInfected = false }
          { Id = "PC5"
            OS = Linux
            IsInfected = false } ]

    let connections =
        [ ("PC1", "PC2")
          ("PC1", "PC4")
          ("PC4", "PC3")
          ("PC4", "PC5") ]

    let rand = Random()
    let checkProbability (p: float) = rand.NextDouble() <= p
    let network = Network(computers, connections, checkProbability)

    let printCurrentState () =
        for item in network.Computers do
            printfn
                "Компьютер %s: %s"
                item.Id
                (if item.IsInfected then
                     "Заражен"
                 else
                     "Не заражен")

        printfn "\n"

    printfn "Начальное состояние"
    printCurrentState ()

    let mutable turn = 1
    let mutable hasChanges = true

    while hasChanges do
        hasChanges <- network.Step()

        if hasChanges then
            printfn "Состояние после %d хода" turn
            printCurrentState()
            turn <- turn + 1

    printf "Эпидемия закончилась"
    0
