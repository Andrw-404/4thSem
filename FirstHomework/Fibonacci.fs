module FibonacciTask

let fibonacci n =
    let rec loop current a b =
        match current with
        | 0 -> a
        | _ -> loop (current - 1) b (a + b)

    if n < 0 then None else Some(loop n 0 1);;
