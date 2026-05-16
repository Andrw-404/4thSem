module BracketSequence

let getMatchingOpenBracket =
    function
    | ')' -> Some '('
    | ']' -> Some '['
    | '}' -> Some '{'
    | _ -> None

let isValidSequence input =
    let rec check chars stack =
        match chars with
        | [] -> List.isEmpty stack
        | current :: rest ->
            match current, getMatchingOpenBracket current, stack with
            | ('(' | '{' | '['), _, _ -> check rest (current :: stack)
            | _, Some expectedOpen, top :: stackRest when top = expectedOpen -> check rest stackRest
            | _, Some _, _ -> false
            | _, None, _ -> check rest stack

    check (Seq.toList input) []