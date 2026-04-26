module FirstHomework.ListConversionTests

open NUnit.Framework
open FsUnit
open ListConversion

[<Test>]
let ``Обращение пустого списка возвращает пустой список`` () =
    reverseList [] |> should equal []

[<Test>]
let ``Обращение списка с одним элементом возвращает тот же список`` () =
    reverseList [1] |> should equal [1]

[<Test>]
let ``Обращение списка работает корректно`` () =
    let input = [1;2;3;4;5]
    let expected = [5;4;3;2;1]
    reverseList input |> should equal expected