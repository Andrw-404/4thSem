module FactorialTask

let factorial n =
    let rec loop current acc =
        match current with
        | 0 -> acc
        | _ -> loop (current - 1) (acc * current)

    if n < 0 then None else Some(loop n 1)
