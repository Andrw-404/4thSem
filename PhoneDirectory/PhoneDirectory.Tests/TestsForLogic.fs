module PhoneDirectory.Tests

open PhoneDirectory.Logic
open NUnit.Framework
open FsUnit

type DbResult = Result<(string * string) list, string>

[<Test>]
let ``add должен возвращать Error, если имя пустое`` () =
    let result = add "" "123456" []

    result
    |> should equal (Error "Имя контакта пустое": DbResult)

[<Test>]
let ``add должен возвращать Error, если телефон пуст`` () =
    let result = add "qwerty" "" []

    result
    |> should equal (Error "Телефон должен содержать только цифры и +": DbResult)

[<Test>]
let ``add должен возвращать Error, если телефон содержит буквы`` () =
    let result = add "qwerty" "123a45" []

    result
    |> should equal (Error "Телефон должен содержать только цифры и +": DbResult)

[<Test>]
let ``add должен возвращать Error, если телефон содержит символы`` () =
    let result = add "qwerty" "123-45436" []

    result
    |> should equal (Error "Телефон должен содержать только цифры и +": DbResult)

[<Test>]
let ``add должен возвращать Ok с новой базой, если данные корректны`` () =
    let result = add "qwerty" "+3213" []

    result
    |> should equal (Ok [ ("qwerty", "+3213") ]: DbResult)

[<Test>]
let ``add должен добавлять новую запись в начало существующей базы`` () =
    let initialDb = [ ("qwerty", "111") ]
    let result = add "zxcv" "222" initialDb

    result
    |> should
        equal
        (Ok [ ("zxcv", "222")
              ("qwerty", "111") ]: DbResult)

[<Test>]
let ``findByName должен возвращать совпадающие записи(имена)`` () =
    let db =
        [ ("zxc", "222")
          ("qwerty", "111")
          ("qwerty", "3333") ]

    let result = findByName "qwerty" db

    result
    |> should
        equal
        [ ("qwerty", "111")
          ("qwerty", "3333") ]

[<Test>]
let ``findByName должен возвращать совпадающие записи(телефоны)`` () =
    let db =
        [ ("zxc", "222")
          ("qwerty", "222")
          ("qwerty", "3333") ]

    let result = findByPhone "222" db

    result
    |> should equal [ ("zxc", "222"); ("qwerty", "222") ]
