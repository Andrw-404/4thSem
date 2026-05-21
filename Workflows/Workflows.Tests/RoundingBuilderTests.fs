module RoundingBuilder.Tests

open NUnit.Framework
open FsUnit
open System
open RoundingBuilder

[<Test>]
let ``rounding корректно вычисляет выражение`` () =
    let result = rounding 3{
        let! a = 2.0 / 12.0
        let! b = 3.5
        return a / b
    }
    result |> should equal 0.048

[<Test>]
let ``rounding с точностью 0 округляет до целого`` () =
    let result = rounding 0{
        let! a = 2.053
        return a
    }
    result |> should equal 2.0

[<Test>]
let ``return должен сам округлять итоговое значение`` () =
    let result = rounding 2{
        return 3.1415
    }
    result |> should equal 3.14

[<Test>]
let ``rounding с отрицательной точностью выбросит исключение`` () =
    let invalidBuilder () = rounding -1 |> ignore
    invalidBuilder |> should throw typeof<ArgumentOutOfRangeException>