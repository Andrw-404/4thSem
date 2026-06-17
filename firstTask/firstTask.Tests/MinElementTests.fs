module findMinValueTask.Tests

open FirstTask
open FsUnit
open NUnit.Framework

[<Test>]
let ``FindMinValue вернет None если список пуст`` () =
    let emptyList: int list = []
    findMinValue emptyList |> should equal None

[<Test>]
let ``FindMinValue корректно работает со списком положительных чисел`` () =
    let list = [ 10; 9; 4; 6; 3; 1 ]
    findMinValue list |> should equal (Some 1)

[<Test>]
let ``FindMinValue корректно работает со списком отрицательных чисел`` () =
    let list = [ -10; -9; -4; -6; -3; -1 ]
    findMinValue list |> should equal (Some -10)

[<Test>]
let ``FindMinValue корректно работает со списком, где значения разных знаков`` () =
    let list = [ -10; 9; 4; -6; -3; 1 ]
    findMinValue list |> should equal (Some -10)
