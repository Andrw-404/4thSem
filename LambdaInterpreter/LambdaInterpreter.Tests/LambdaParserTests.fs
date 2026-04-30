module LambdaParser.Tests

open NUnit.Framework
open FsUnit
open LambdaParser

[<Test>]
let ``Parser S K K эквивалентен \\x.x`` () =
    let input = "let S = \\x y z.x z (y z)\n\nlet K = \\x y.x\n\nS K K"

    let result = runString input
    result |> should equal "\\z.z"

[<Test>]
let ``Parser применение тождественной функции к переменной`` () =
    let input = "(\\x.x) y"

    let result = runString input
    result |> should equal "y"

[<Test>]
let ``Parser лямбда с двумя параметрами и каррирование`` () =
    let input = "(\\x y.x) A B"

    let result = runString input
    result |> should equal "A"

[<Test>]
let ``Parser let определение подставляется в основное выражение`` () =
    let input = "let I = \\x.x\n\nI z"

    let result = runString input
    result |> should equal "z"

[<Test>]
let ``Parser комбинатор K примененный к двум аргументам`` () =
    let input = "let K = \\x y.x\n\nK p q"

    let result = runString input
    result |> should equal "p"

[<Test>]
let ``Parser допускает пробелы вокруг равно, скобок и между атомами`` () =
    let input = "let  K  =  \\x y.x\n\n(  K  )  p   q"

    let result = runString input
    result |> should equal "p"

[<Test>]
let ``Parser допускает несколько пустых строк между let и основным термом`` () =
    let input = "let I = \\x.x\n\n\n\n\n\nI y"

    let result = runString input
    result |> should equal "y"