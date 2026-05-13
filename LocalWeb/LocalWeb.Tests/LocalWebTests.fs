module LocalWeb.Tests

open NUnit.Framework
open FsUnit
open Network
open Computer

[<Test>]
let ``Вирус не передается без соединений`` () =
    let pc1 = Computer("PC1", Windows, true)
    let pc2 = Computer("PC2", Windows, false)

    let computers = [ pc1; pc2 ]
    let connections = []

    let alwaysTrue = fun _ -> true
    let network = Network(computers, connections, alwaysTrue)

    network.Step() |> should be False
    pc2.IsInfected |> should be False

[<Test>]
let ``При вероятности 1 обход в ширину`` () =
    let pc1 = Computer("PC1", Windows, true)
    let pc2 = Computer("PC2", Windows, false)
    let pc3 = Computer("PC3", Windows, false)

    let computers = [ pc1; pc2; pc3 ]
    let connections = [ (pc1, pc2); (pc2, pc3) ]

    let alwaysTrue = fun _ -> true

    let network = Network(computers, connections, alwaysTrue)

    network.Step() |> should be True
    pc2.IsInfected |> should be True
    pc3.IsInfected |> should be False

    network.Step() |> should be True
    pc3.IsInfected |> should be True

    network.Step() |> should be False

[<Test>]
let ``При вероятности 0 никто не заражается`` () =
    let pc1 = Computer("PC1", Windows, true)
    let pc2 = Computer("PC2", Windows, false)

    let computers = [ pc1; pc2 ]
    let connections = [ (pc1, pc2) ]

    let alwaysFalse = fun _ -> false

    let network = Network(computers, connections, alwaysFalse)

    network.Step() |> should be False
    pc2.IsInfected |> should be False
