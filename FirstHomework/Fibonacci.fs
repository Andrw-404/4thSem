module FibonacciTask

let fibonacci n =
    let rec factorialHelper current prev curr =
        match current with
        | 0 -> prev
        | _ -> factorialHelper (current - 1) curr (prev + curr)

    if n < 0 then
        None
    else
        Some(factorialHelper n 0 1)