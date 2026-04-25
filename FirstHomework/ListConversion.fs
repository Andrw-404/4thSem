module ListConversion

let reverseList list =
    let rec reverseTailRec acc currentList = 
        match currentList with
        | [] -> acc
        | head :: tail -> reverseTailRec (head :: acc) tail

    reverseTailRec [] list