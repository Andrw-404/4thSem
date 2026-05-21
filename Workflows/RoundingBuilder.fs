module RoundingBuilder

open System

type RoundingBuilder (precision) =
    do ArgumentOutOfRangeException.ThrowIfNegative precision
    member this.Bind(value:float, f) =
        Math.Round(value, precision) |> f

    member this.Return (x: float) = Math.Round(x, precision)

let rounding precision = RoundingBuilder(precision)