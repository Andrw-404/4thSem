module NumberSearch

let findNumber target list =
    let rec search index =
        function
        | [] -> None
        | head :: _ when head = target -> Some index
        | _ :: tail -> search (index + 1) tail

    search 0 list