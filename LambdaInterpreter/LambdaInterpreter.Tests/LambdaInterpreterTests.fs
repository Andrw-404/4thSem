module LambdaInterpreter.Tests

open NUnit.Framework
open FsUnit
open LambdaInterpreter

[<Test>]
let ``FindFV в выражении λx.xy свободна y`` () =
    let term = LambdaAbstraction("x", Application(Variable "x", Variable "y"))
    findFV term |> should equal (Set.singleton "y")

[<Test>]
let ``FindFV в выражении λx.x нет свободных переменных`` () =
    let term = LambdaAbstraction("x", Variable "x")
    findFV term |> should equal Set.empty<string>

[<Test>]
let ``Substitute в выражении (x y) замена y на z дает (x z)`` () =
    let term = Application(Variable "x", Variable "y")
    let replacement = Variable "z"

    substitute "y" replacement term
    |> should equal (Application(Variable "x", Variable "z"))

[<Test>]
let ``Substitute альфа-преобразование корректно обрабатывает зависимую переменную`` () =
    let term = LambdaAbstraction("x", Variable "y")
    let replacement = Variable "x"
    let expected = LambdaAbstraction("x1", Variable "x")

    substitute "y" replacement term
    |> should equal expected

[<Test>]
let ``Evaluate (λx.x)y дает y`` () =
    let term = Application(LambdaAbstraction("x", Variable "x"), Variable "y")
    evaluate term |> should equal (Variable "y")

[<Test>]
let ``Evaluate использует нормальную стратегию для обработки сложных выражений`` () =
    let tripleX =
        LambdaAbstraction("x", Application(Application(Variable "x", Variable "x"), Variable "x"))

    let monster = Application(tripleX, tripleX)
    let term = Application(LambdaAbstraction("x", Variable "y"), monster)
    evaluate term |> should equal (Variable "y")
