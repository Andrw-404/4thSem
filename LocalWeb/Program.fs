open Computer
open Network
open System

let pc1 = Computer("PC1", Windows, false)
let pc2 = Computer("PC2", MacOS, true)
let pc3 = Computer("PC3", Linux, false)
let pc4 = Computer("PC4", Windows, false)
let pc5 = Computer("PC5", Linux, false)

let computers = [ pc1; pc2; pc3; pc4; pc5 ]

let connections =
    [ (pc1, pc2)
      (pc1, pc4)
      (pc4, pc3)
      (pc4, pc5) ]

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

while network.CanStateChange do
    network.Step() |> ignore

    printfn "Состояние после %d хода" turn
    printCurrentState ()
    turn <- turn + 1

printf "Эпидемия закончилась"
