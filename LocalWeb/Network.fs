module Network

open Computer

type Connection = string * string

type Network(initialComputers: list<Computer>, connections: list<Connection>, probability: Probability) =
    let mutable currentComputers =
        initialComputers
        |> List.map (fun pc -> pc.Id, pc)
        |> Map.ofList

    let addEdge m a b =
        let m1 =
            let tail = Map.tryFind a m |> Option.defaultValue []
            Map.add a (b :: tail) m

        let tailB = Map.tryFind b m1 |> Option.defaultValue []
        Map.add b (a :: tailB) m1

    let neighborsById =
        connections
        |> List.fold (fun m (a, b) -> addEdge m a b) Map.empty

    let neighborsOf (id: string) =
        Map.tryFind id neighborsById
        |> Option.defaultValue []

    let mutable frontier =
        initialComputers
        |> List.filter (fun c -> c.IsInfected)
        |> List.map (fun c -> c.Id)
        |> Set.ofList

    member this.Computers = currentComputers.Values |> Seq.toList

    member this.Step() =
        if Set.isEmpty frontier then
            false
        else
            let newInfected =
                frontier
                |> Seq.collect neighborsOf
                |> Seq.distinct
                |> Seq.choose (fun id ->
                    let pc = currentComputers.[id]

                    if not pc.IsInfected
                       && probability pc.OS.InfectionProbability then
                        Some pc
                    else
                        None)
                |> Seq.toList

            for pc in newInfected do
                currentComputers <- currentComputers.Add(pc.Id, pc.Infect())

            frontier <-
                newInfected
                |> List.map (fun c -> c.Id)
                |> Set.ofList

            not (List.isEmpty newInfected)
