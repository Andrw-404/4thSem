module NumberSearch

let findNumber target list =
    let rec search index currentList =
        match currentList with
        | [] -> None
        | head :: _ when head = target -> Some index
        | _ :: tail -> search (index + 1) tail

    search 0 list