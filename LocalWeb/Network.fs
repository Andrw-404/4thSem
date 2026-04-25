module Network

open Computer

type Connection = Computer * Computer

type Network(computers: list<Computer>, connections: list<Connection>, probability: Probability) =

    let addEdge neighborsMap (firstComp: Computer) (secondComp: Computer) =
        let firstCompNeighbors =
            Map.tryFind firstComp.Id neighborsMap
            |> Option.defaultValue []

        let mapWithFirstCompUpd =
            Map.add firstComp.Id (secondComp :: firstCompNeighbors) neighborsMap

        let secondCompNeighbors =
            Map.tryFind secondComp.Id mapWithFirstCompUpd
            |> Option.defaultValue []

        Map.add secondComp.Id (firstComp :: secondCompNeighbors) mapWithFirstCompUpd

    let neighborsById =
        connections
        |> List.fold (fun acc (firstComp, secondComp) -> addEdge acc firstComp secondComp) Map.empty

    let neighborsOf (pc: Computer) =
        Map.tryFind pc.Id neighborsById
        |> Option.defaultValue []

    member this.Computers = computers

    member this.CanStateChange =
        computers
        |> Seq.filter (fun c -> c.IsInfected)
        |> Seq.collect neighborsOf
        |> Seq.exists (fun neighbor -> not neighbor.IsInfected)

    member this.Step() =
        let infectedComputers = computers |> List.filter (fun c -> c.IsInfected)

        let newlyInfected =
            infectedComputers
            |> Seq.collect neighborsOf
            |> Seq.distinctBy (fun c -> c.Id)
            |> Seq.filter (fun c -> not c.IsInfected)
            |> Seq.filter (fun c -> probability c.OS.InfectionProbability)
            |> Seq.toList

        for pc in newlyInfected do
            pc.Infect()

        not (List.isEmpty newlyInfected)
