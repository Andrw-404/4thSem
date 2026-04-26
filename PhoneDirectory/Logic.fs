module PhoneDirectory.Logic

type Name = Name of string
type Phone = Phone of string
type Record = { Name: Name; Phone: Phone }
type PhoneBook = Record list

let emptyData: PhoneBook = []

let add (nameStr: string) (phoneStr: string) (db: PhoneBook) : Result<PhoneBook, string> =
    if nameStr = "" then
        Error "Имя контакта пустое"
    elif
        phoneStr = ""
        || not (Seq.forall System.Char.IsDigit (phoneStr.Replace("+", "")))
    then
        Error "Телефон должен содержать только цифры и +"
    else
        let newRecord =
            { Name = Name nameStr
              Phone = Phone phoneStr }

        Ok(newRecord :: db)

let findByName (searchName: string) (db: PhoneBook) : Record list =
    db
    |> List.filter (fun { Name = Name n } -> n = searchName)

let findByPhone (searchPhone: string) (db: PhoneBook) : Record list =
    db
    |> List.filter (fun { Phone = Phone p } -> p = searchPhone)

let formatContacts (db: PhoneBook) : string =
    match db with
    | [] -> "Список контактов пуст"
    | _ ->
        db
        |> List.map (fun { Name = Name n; Phone = Phone p } -> sprintf "%s: %s" n p)
        |> String.concat "\n"
