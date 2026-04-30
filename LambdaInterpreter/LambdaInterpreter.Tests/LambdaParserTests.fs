module LambdaParser.Tests

open NUnit.Framework
open FsUnit
open LambdaParser

[<Test>]
let ``Parser S K K эквивалентен \\x.x`` () =
    let input = "let S = \\x y z.x z (y z)\n\nlet K = \\x y.x\n\nS K K"

    let result = runString input
    result |> should equal "\\z.z"