open System
open PhoneDirectory.Logic
open PhoneDirectory.FileLogic

let rec reader request =
    printf "%s" request

    match Console.ReadLine() with
    | null
    | "" ->
        printfn "Ввод не может быть пустым"
        reader request
    | s -> s.Trim()

let rec mainLoop (db: PhoneBook) =
    printfn "\n1. Показать всё"
    printfn "2. Добавить запись (имя и телефон)"
    printfn "3. Найти телефон по имени"
    printfn "4. Найти имя по телефону"
    printfn "5. Сохранить текущие данные в файл"
    printfn "6. Считать данные из файла"
    printfn "7. Выйти"

    match reader "\nВыберите действие: " with
    | "1" ->
        printfn "%s" (formatContacts db)
        mainLoop db

    | "2" ->
        let name = reader "Имя: "
        let phone = reader "Телефон: "

        match add name phone db with
        | Ok newDb ->
            printfn "Контакт добавлен"
            mainLoop newDb
        | Error err ->
            printfn "Ошибка: %s" err
            mainLoop db

    | "3" ->
        let name = reader "Имя для поиска: "

        match findByName name db with
        | [] -> printfn "Не найдено"
        | results ->
            results
            |> List.iter (fun { Phone = Phone p } -> printfn "%s" p)

        mainLoop db

    | "4" ->
        let phone = reader "Телефон для поиска: "

        match findByPhone phone db with
        | [] -> printfn "Не найдено"
        | results ->
            results
            |> List.iter (fun { Name = Name n } -> printfn "%s" n)

        mainLoop db

    | "5" ->
        let path = reader "Путь для сохранения: "

        match save path db with
        | Ok () -> printfn "Сохранено"
        | Error err -> printfn "Ошибка при сохранении: %s" err

        mainLoop db

    | "6" ->
        let path = reader "Путь для загрузки: "

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

printfn "Телефонный справочник"
mainLoop emptyData