module FirstHomework.FindNumberTests

open NUnit.Framework
open FsUnit
open NumberSearch

[<Test>]
let ``Поиск в пустом списке возвращает None`` () =
    findNumber 2 [] |> should equal None

[<Test>]
let ``Если элемента нет в списке, возвращается None`` () =
    let input = [2;3;4;5]
    findNumber 1 input |> should equal None

[<Test>]
let ``Индекс вхождения корректно высчитывается`` () =
    let input = [1;2;3;4;5]
    findNumber 3 input |> should equal (Some 2)

[<Test>]
let ``Если элемент встречается несколько раз, то возвращается индекс первого вхождения`` () =
    let input = [1;2;3;2;4;5]
    findNumber 2 input |> should equal (Some 1)