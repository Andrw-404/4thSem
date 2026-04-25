module Computer

type Probability = float -> bool

type OS =
    | Windows
    | Linux
    | MacOS
    member this.InfectionProbability =
        match this with
        | Windows -> 0.5
        | Linux -> 0.3
        | MacOS -> 0.2

type Computer(id: string, os: OS, isInfected: bool) =
    let mutable _isInfected = isInfected
    member this.Id = id
    member this.OS = os

    member this.IsInfected
        with get () = _isInfected
        and set (x) = _isInfected <- x

    member this.Infect() = this.IsInfected <- true
