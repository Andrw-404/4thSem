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

type Computer =
    { Id: string
      OS: OS
      IsInfected: bool }
    member this.Infect() =
        if this.IsInfected then
            this
        else
            { this with IsInfected = true }
