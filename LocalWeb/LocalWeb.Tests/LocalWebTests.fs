module LocalWeb.Tests

open NUnit.Framework
open FsUnit
open Network
open Computer

[<TestFixture>]
type ``Tests``() =

    [<Test>]
    member this.``Вирус не передается без соединений``() =
        let computers = [
            { Id = "PC1"; OS = Windows; IsInfected = true }
            { Id = "PC2"; OS = Windows; IsInfected = false }
        ]
        let connections = []
        
        let alwaysTrue = fun _ -> true
        let network = Network(computers, connections, alwaysTrue)
        
        network.Step() |> should be False
        let pc2 = network.Computers |> List.find (fun c -> c.Id = "PC2")
        pc2.IsInfected |> should be False

    [<Test>]
    member this.``При вероятности 1 обход в ширину``() =
        let computers = [
            { Id = "PC1"; OS = Windows; IsInfected = true }
            { Id = "PC2"; OS = Windows; IsInfected = false }
            { Id = "PC3"; OS = Windows; IsInfected = false }
        ]
        let connections = [ ("PC1", "PC2"); ("PC2", "PC3") ]
        
        let alwaysTrue = fun _ -> true
        
        let network = Network(computers, connections, alwaysTrue)
        
        network.Step() |> should be True
        let state1 = network.Computers |> List.sortBy (fun c -> c.Id)
        state1.[1].IsInfected |> should be True
        state1.[2].IsInfected |> should be False

        network.Step() |> should be True
        let state2 = network.Computers |> List.sortBy (fun c -> c.Id)
        state2.[2].IsInfected |> should be True

        network.Step() |> should be False

    [<Test>]
    member this.``При вероятности 0 никто не заражается``() =
        let computers = [
            { Id = "PC1"; OS = Windows; IsInfected = true }
            { Id = "PC2"; OS = Windows; IsInfected = false }
        ]
        let connections = [ ("PC1", "PC2") ]
        
        let alwaysFalse = fun _ -> false
        
        let network = Network(computers, connections, alwaysFalse)
        
        network.Step() |> should be False
        let pc2 = network.Computers |> List.find (fun c -> c.Id = "PC2")
        pc2.IsInfected |> should be False