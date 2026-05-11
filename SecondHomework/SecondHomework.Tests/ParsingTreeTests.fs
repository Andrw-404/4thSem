module SecondHomework.ParsingTreeTests

open NUnit.Framework
open ParsingTreeTask
open FsUnit

[<Test>]
let ``Вычисление числа возвращает само число`` () = calculate (Value 5) |> should equal 5

[<Test>]
let ``Сложение корректно складывает два значения`` () =
    calculate (Add(Value 5, Value 3)) |> should equal 8

[<Test>]
let ``Вычитание корректно вычитает правое из левого`` () =
    calculate (Sub(Value 5, Value 3)) |> should equal 2

[<Test>]
let ``Умножение корректно перемножает два значения`` () =
    calculate (Mul(Value 5, Value 3)) |> should equal 15

[<Test>]
let ``Деление корректно делит левое значение на правое`` () =
    calculate (Div(Value 10, Value 2)) |> should equal 5

[<Test>]
let ``Сложное выражение корректно вычисляется`` () =
    let expr = Div(Mul(Add(Value 2, Value 3), Value 9), Value 15)
    calculate expr |> should equal 3
