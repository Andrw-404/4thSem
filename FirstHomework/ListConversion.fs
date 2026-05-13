module ListConversion

let reverseList list =
    let rec reverseTailRec acc =
        function
        | [] -> acc
        | head :: tail -> reverseTailRec (head :: acc) tail

    reverseTailRec [] list