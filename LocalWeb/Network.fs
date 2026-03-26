module Network

open Computer

type Connection = string * string

type Network(initialComputers: list<Computer>, connections: list<Connection>, probability: Probability) =
    let mutable currentComputers =
        initialComputers
        |> List.map (fun pc -> pc.Id, pc)
        |> Map.ofList

    let getNeighbors (id: string) =
        connections
        |> List.choose (fun (a, b) ->
            if a = id then Some b
            elif b = id then Some a
            else None)

    member this.Computers = currentComputers.Values |> Seq.toList

    member this.Step() =
        let sickPC =
            currentComputers.Values
            |> Seq.filter (fun c -> c.IsInfected)

        let NeighborsAtRisk = 
            sickPC
            |> Seq.collect (fun c -> getNeighbors c.Id)
            |> Seq.distinct
            |> Seq.map (fun id -> currentComputers.[id])
            |> Seq.filter (fun c -> not c.IsInfected)

        let newInfected = 
            NeighborsAtRisk 
            |> Seq.filter (fun pc -> probability(pc.OS.InfectionProbability)) 
            |> Seq.toList

        for pc in newInfected do 
            let sickPC = pc.Infect()
            currentComputers <- currentComputers.Add(pc.Id, sickPC)

        not (List.isEmpty newInfected)