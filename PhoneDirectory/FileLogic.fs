module PhoneDirectory.FileLogic

open System.IO

let DBToFile (db: (string * string) list) : string =
    db
    |> List.map (fun (name, phone) -> sprintf "%s, %s" name phone)
    |> String.concat "\n"

let fileToDB (content: string) : (string * string) list =
    content.Split('\n', System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map (fun line ->
        let parts = line.Split(',')

        if parts.Length = 2 then
            (parts.[0].Trim(), parts.[1].Trim())
        else
            ("", ""))
    |> List.filter (fun (name, phone) -> name <> "" && phone <> "")

let load (path: string) : Result<(string * string) list, string> =
    try
        let text = File.ReadAllText(path)
        Ok(fileToDB text)
    with
    | :? FileNotFoundException -> Error "Файл не найден"
    | ex -> Error ex.Message

let save (path: string) (db: (string * string) list) : Result<unit, string> =
    try
        File.WriteAllText(path, DBToFile db)
        Ok()
    with
    | ex -> Error("Не удалось сохранить файл " + ex.Message)
