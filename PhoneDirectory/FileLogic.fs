module PhoneDirectory.FileLogic

open PhoneDirectory.Logic
open System.IO
open System

let DBToFile (db: PhoneBook) : string =
    db
    |> List.map (fun { Name = Name n; Phone = Phone p } -> sprintf "%s, %s" n p)
    |> String.concat "\n"

let fileToDB (content: string) : Result<PhoneBook, string> =
    let lines =
        content.Split([| '\n' |], System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList

    let rec parserLines (acc: PhoneBook) (remainingLines: string list) =
        match remainingLines with
        | [] -> Ok(List.rev acc)
        | line :: rest ->
            match line.Split(',') with
            | [| name; phone |] ->
                let record =
                    { Name = Name(name.Trim())
                      Phone = Phone(phone.Trim()) }

                parserLines (record :: acc) rest
            | _ -> Error "Неверный формат файла"

    parserLines [] lines

let load (path: string) : Result<PhoneBook, string> =
    try
        let text = File.ReadAllText(path)
        fileToDB text
    with
    | :? FileNotFoundException -> Error "Файл не найден"
    | ex -> Error ex.Message

let save (path: string) (db: PhoneBook) : Result<unit, string> =
    try
        File.WriteAllText(path, DBToFile db)
        Ok()
    with
    | ex -> Error("Не удалось сохранить файл " + ex.Message)
