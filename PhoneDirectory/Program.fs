open System
open PhoneDirectory.Logic
open PhoneDirectory.FileLogic

let reader request =
    printf "%s" request

    match Console.ReadLine() with
    | null -> ""
    | s -> s.Trim()

let rec mainLoop (db: (string * string) list) =
    printfn "\n1. Показать всё"
    printfn "2. Добавить запись (имя и телефон)"
    printfn "3. Найти телефон по имени"
    printfn "4. Найти имя по телефону"
    printfn "5. Сохранить текущие данные в файл"
    printfn "6. Считать данные из файла"
    printfn "7. Выйти"

    match reader "\nВыберите действие: " with
    | "1" ->
        match db with
        | [] -> printfn "Список контактов пуст"
        | _ ->
            db
            |> List.iter (fun (name, phone) -> printfn "%s: %s" name phone)

        mainLoop db

    | "2" ->
        match reader "Имя: " with
        | "" ->
            printfn "Имя не указано"
            mainLoop db
        | name ->
            match reader "Телефон: " with
            | "" ->
                printfn "Телефон не указан"
                mainLoop db
            | phone ->
                match add name phone db with
                | Ok newDb ->
                    printfn "Контакт добавлен"
                    mainLoop newDb
                | Error err ->
                    printfn "Ошибка: %s" err
                    mainLoop db

    | "3" ->
        match reader "Имя для поиска: " with
        | "" ->
            printfn "Имя не указано"
            mainLoop db
        | name ->
            let results = findByName name db

            if List.isEmpty results then
                printfn "Не найдено"
            else
                results
                |> List.iter (fun (_, phone) -> printfn "%s" phone)

            mainLoop db

    | "4" ->
        match reader "Телефон для поиска: " with
        | "" ->
            printfn "Телефон не указан"
            mainLoop db
        | phone ->
            let results = findByPhone phone db

            if List.isEmpty results then
                printfn "Не найдено"
            else
                results
                |> List.iter (fun (name, _) -> printfn "%s" name)

            mainLoop db

    | "5" ->
        match reader "Путь для сохранения: " with
        | "" ->
            printfn "Путь не указан"
            mainLoop db
        | path ->
            match save path db with
            | Ok () -> printfn "Сохранено"
            | Error err -> printfn "Ошибка при сохранении: %s" err

            mainLoop db

    | "6" ->
        match reader "Путь для загрузки: " with
        | "" ->
            printfn "Путь не указан"
            mainLoop db
        | path ->
            match load path with
            | Ok newDb ->
                printfn "Загружено"
                mainLoop newDb
            | Error err ->
                printfn "Ошибка при загрузке: %s" err
                mainLoop db

    | "7" -> printfn "Выход..."

    | _ ->
        printfn "Неверный выбор, попробуйте снова"
        mainLoop db

[<EntryPoint>]
let main _argv =
    printfn "Телефонный справочник"
    mainLoop emptyData
    0