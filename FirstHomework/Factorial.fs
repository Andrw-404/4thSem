module FactorialTask

let factorial n =
    let rec factorialHelper current acc =
        match current with
        | 0 -> acc
        | _ -> factorialHelper (current - 1) (acc * current)

    if n < 0 then None else Some(factorialHelper  n 1)
