module PhoneDirectory.TestsForFileLogic

open PhoneDirectory.FileLogic
open NUnit.Framework
open FsUnit
open System.IO

type Db = (string * string) list
type DbResult = Result<(string * string) list, string>
type SaveResult = Result<unit, string>

[<Test>]
let ``DBToFile пустой список в пустую строку`` () = DBToFile [] |> should equal ""

[<Test>]
let ``DBToFile несколько элементов в строку`` () =
    let db = [ ("qwerty", "+123"); ("zxcv", "456") ]

    DBToFile db
    |> should equal "qwerty, +123\nzxcv, 456"

[<Test>]
let ``fileToDB пустая строка в пустой список`` () = fileToDB "" |> should equal ([]: Db)

[<Test>]
let ``fileToDB корректный ввод превращается в список`` () =
    fileToDB "qwerty, +123\nzxcv, 456"
    |> should equal ([ ("qwerty", "+123"); ("zxcv", "456") ]: Db)

[<Test>]
let ``fileToDB строки без запятой отбрасываются`` () =
    fileToDB "qwerty +123\nzxcv, 456"
    |> should equal ([ ("zxcv", "456") ]: Db)

[<Test>]
let ``fileToDB строки с лишними запятыми отбрасываются`` () =
    fileToDB "qwerty, +123, выфвфы\nzxcv, 456"
    |> should equal ([ ("zxcv", "456") ]: Db)

[<Test>]
let ``load корректный файл`` () =
    let path = Path.GetTempFileName()

    try
        File.WriteAllText(path, "qwerty, +123\nzxcv, 456")

        load path
        |> should
            equal
            (Ok [ ("qwerty", "+123")
                  ("zxcv", "456") ]: DbResult)
    finally
        File.Delete(path)

[<Test>]
let ``save корректно записывает данные`` () =
    let path = Path.GetTempFileName()

    try
        let db = [ ("qwerty", "+123"); ("zxcv", "456") ]
        save path db |> should equal (Ok(): SaveResult)

        File.ReadAllText(path)
        |> should equal "qwerty, +123\nzxcv, 456"
    finally
        File.Delete(path)
