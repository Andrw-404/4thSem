module ParenthesisSequence.Tests

open NUnit.Framework
open BracketSequence
open FsUnit

[<Test>]
let ``Пустая строка обрабатывается корректно`` () =
    isValidSequence "" |> should be True

[<Test>]
let ``Несложные пары скобок корректно обрабатываются`` () =
    isValidSequence "()" |> should be True
    isValidSequence "[]" |> should be True
    isValidSequence "{}" |> should be True

[<Test>]
let ``Сложная вложенность корректно обрабатывается`` () =
    isValidSequence "({[]})" |> should be True

[<Test>]
let ``Наличие текста между скобками не мешает`` () =
    isValidSequence "(dsad[]fds){vd=s}" |> should be True

[<Test>]
let ``Неправильный порядок скобок возвращает false`` () =
    isValidSequence "{)[}]" |> should be False

[<Test>]
let ``Незакрытые скобки корректно обрабатываются`` () =
    isValidSequence "(((" |> should be False

[<Test>]
let ``Лишняя закрывающая скобка корректно обрабатываются`` () =
    isValidSequence "())" |> should be False