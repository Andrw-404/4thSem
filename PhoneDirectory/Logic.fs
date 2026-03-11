module PhoneDirectory.Logic

let emptyData: (string * string) list = []

let add (name: string) (phone: string) (db: (string * string) list) : Result<(string * string) list, string> =
    if name = "" then
        Error "Имя контакта пустое"
    elif not (Seq.forall System.Char.IsDigit (phone.Replace("+", ""))) then
        Error "Телефон должен содержать только цифры и +"
    else
        Ok((name, phone) :: db)

let findByName (name: string) : (string * string) list -> (string * string) list = List.filter ((=) name << fst)

let findByPhone (phone: string) : (string * string) list -> (string * string) list = List.filter ((=) phone << snd)
