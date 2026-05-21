module StringCalculateBuilder

open System

let tryParseInt (str: string) =
    match Int32.TryParse str with
    | true, value -> Some value
    | false, _ -> None

type CalculateBuilder()=
    member this.Bind(value, f) =
        match tryParseInt value with
        | Some num -> f num
        | None -> None

    member this.Return x = Some x
    
let calculate = CalculateBuilder()