module PhoneDirectory.TestsForFileLogic

open PhoneDirectory.Logic
open PhoneDirectory.FileLogic
open NUnit.Framework
open FsUnit
open System.IO

[<Test>]
let ``DBToFile пустой список в пустую строку`` () = DBToFile [] |> should equal ""

[<Test>]
let ``DBToFile несколько элементов в строку`` () =
    let db =
        [ { Name = Name "qwerty"
            Phone = Phone "+123" }
          { Name = Name "zxcv"
            Phone = Phone "456" } ]

    DBToFile db
    |> should equal "qwerty, +123\nzxcv, 456"

[<Test>]
let ``fileToDB пустая строка в пустой список`` () =
    fileToDB ""
    |> should equal (Ok []: Result<PhoneBook, string>)

[<Test>]
let ``fileToDB корректный ввод превращается в список`` () =
    fileToDB "qwerty, +123\nzxcv, 456"
    |> should
        equal
        (Ok [ { Name = Name "qwerty"
                Phone = Phone "+123" }
              { Name = Name "zxcv"
                Phone = Phone "456" } ]: Result<PhoneBook, string>)

[<Test>]
let ``fileToDB строки без запятой отбрасываются с ошибкой`` () =
    fileToDB "qwerty +123\nzxcv, 456"
    |> should equal (Error "Неверный формат файла": Result<PhoneBook, string>)

[<Test>]
let ``fileToDB строки с лишними запятыми отбрасываются с ошибкой`` () =
    fileToDB "qwerty, +123, выфвфы\nzxcv, 456"
    |> should equal (Error "Неверный формат файла": Result<PhoneBook, string>)

[<Test>]
let ``load корректный файл`` () =
    let path = Path.GetTempFileName()

    try
        File.WriteAllText(path, "qwerty, +123\nzxcv, 456")

        load path
        |> should
            equal
            (Ok [ { Name = Name "qwerty"
                    Phone = Phone "+123" }
                  { Name = Name "zxcv"
                    Phone = Phone "456" } ]: Result<PhoneBook, string>)
    finally
        File.Delete(path)

[<Test>]
let ``save корректно записывает данные`` () =
    let path = Path.GetTempFileName()

    try
        let db =
            [ { Name = Name "qwerty"
                Phone = Phone "+123" }
              { Name = Name "zxcv"
                Phone = Phone "456" } ]

        save path db |> should equal (Ok() : Result<unit, string>)

        File.ReadAllText(path)
        |> should equal "qwerty, +123\nzxcv, 456"
    finally
        File.Delete(path)
