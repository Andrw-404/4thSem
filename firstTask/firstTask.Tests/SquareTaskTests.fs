module SquereTask.Tests

open SquareTask
open FsUnit
open NUnit.Framework

[<Test>]
let ``getSquare корректно обрабатывает квадрат со сторонами равными 4м`` () =
    let expected = [ "****"; "*  *"; "*  *"; "****" ]
    getSquare 4 |> should equal expected

[<Test>]
let ``getSquare корректно обрабатывает квадрат со сторонами равными 6и`` () =
    let expected =
        [ "******"
          "*    *"
          "*    *"
          "*    *"
          "*    *"
          "******" ]

    getSquare 6 |> should equal expected

[<Test>]
let ``getSquare корректно обрабатывает квадрат со сторонами равными 2м`` () =
    let expected = [ "**"; "**" ]
    getSquare 2 |> should equal expected

[<Test>]
let ``getSquare возвращает одну звездочку при n равном 1`` () =
    let expected = [ "*" ]
    getSquare 1 |> should equal expected

[<Test>]
let ``getSquare возвращает пустой список при n равном 0`` () =
    getSquare 0 |> should equal (([]: string list))

[<Test>]
let ``getSquare возвращает пустой список при отрицательных n`` () =
    getSquare -10 |> should equal ([]: string list)
