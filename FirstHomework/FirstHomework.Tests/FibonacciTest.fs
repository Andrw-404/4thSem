namespace FirstHomework.Tests

module FibonacciTest =

    open NUnit.Framework
    open FsUnit
    open FibonacciTask

    [<TestCase(0, 0)>]
    [<TestCase(1, 1)>]
    [<TestCase(2, 1)>]
    [<TestCase(3, 2)>]
    [<TestCase(15, 610)>]
    [<TestCase(30, 832040)>]
    let ``Провера вычисления чисел Фибоначчи`` (input, expected) = fibonacci input |> should equal (Some expected)

    [<Test>]
    let ``Проверка вычисления чисел Фибоначчи с отрицательными индексами`` () = 
        fibonacci -1 |> should equal None
        fibonacci -100 |> should equal None