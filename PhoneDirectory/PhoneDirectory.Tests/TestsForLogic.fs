module PhoneDirectory.Tests

open PhoneDirectory.Logic
open NUnit.Framework
open FsUnit

[<Test>]
let ``add должен возвращать Error, если имя пустое`` () =
    add "" "123456" []
    |> should equal (Error "Имя контакта пустое": Result<PhoneBook, string>)

[<Test>]
let ``add должен возвращать Error, если телефон пуст`` () =
    add "qwerty" "" []
    |> should equal (Error "Телефон должен содержать только цифры и +": Result<PhoneBook, string>)

[<Test>]
let ``add должен возвращать Error, если телефон содержит буквы`` () =
    add "qwerty" "123a45" []
    |> should equal (Error "Телефон должен содержать только цифры и +": Result<PhoneBook, string>)

[<Test>]
let ``add должен возвращать Error, если телефон содержит символы`` () =
    add "qwerty" "123-45436" []
    |> should equal (Error "Телефон должен содержать только цифры и +": Result<PhoneBook, string>)

[<Test>]
let ``add должен возвращать Ok с новой базой, если данные корректны`` () =
    add "qwerty" "+3213" []
    |> should
        equal
        (Ok [ { Name = Name "qwerty"
                Phone = Phone "+3213" } ]: Result<PhoneBook, string>)

[<Test>]
let ``add должен добавлять новую запись в начало существующей базы`` () =
    let initialDb =
        [ { Name = Name "qwerty"
            Phone = Phone "111" } ]

    add "zxcv" "222" initialDb
    |> should
        equal
        (Ok [ { Name = Name "zxcv"
                Phone = Phone "222" }
              { Name = Name "qwerty"
                Phone = Phone "111" } ]: Result<PhoneBook, string>)

[<Test>]
let ``findByName должен возвращать совпадающие записи(имена)`` () =
    let db =
        [ { Name = Name "zxc"
            Phone = Phone "222" }
          { Name = Name "qwerty"
            Phone = Phone "111" }
          { Name = Name "qwerty"
            Phone = Phone "3333" } ]

    findByName "qwerty" db
    |> should
        equal
        [ { Name = Name "qwerty"
            Phone = Phone "111" }
          { Name = Name "qwerty"
            Phone = Phone "3333" } ]

[<Test>]
let ``findByName должен возвращать совпадающие записи(телефоны)`` () =
    let db =
        [ { Name = Name "zxc"
            Phone = Phone "222" }
          { Name = Name "qwerty"
            Phone = Phone "222" }
          { Name = Name "qwerty"
            Phone = Phone "3333" } ]

    findByPhone "222" db
    |> should
        equal
        [ { Name = Name "zxc"
            Phone = Phone "222" }
          { Name = Name "qwerty"
            Phone = Phone "222" } ]
