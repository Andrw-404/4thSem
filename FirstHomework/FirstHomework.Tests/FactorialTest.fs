module FirstHomework.FactorialTests

open NUnit.Framework
open FsUnit
open FactorialTask

[<TestCase(0, 1)>]
[<TestCase(1, 1)>]
[<TestCase(5, 120)>]
[<TestCase(10, 3628800)>]
let ``Провера факториала различных чисел`` (input, expected) = factorial input |> should equal (Some expected)

[<Test>]
let ``Проверка факториала отрицательных чисел`` () = 
    factorial -1 |> should equal None
    factorial -100 |> should equal None