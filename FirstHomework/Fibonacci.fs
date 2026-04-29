module FibonacciTask

let fibonacci n =
    let rec fibonacciHelper current prev curr =
        match current with
        | 0 -> prev
        | _ -> fibonacciHelper (current - 1) curr (prev + curr)

    if n < 0 then
        None
    else
        Some(fibonacciHelper n 0 1)