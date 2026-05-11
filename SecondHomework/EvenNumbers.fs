module EvenNumbersTask

let countEvenMap list =
    list |> List.map (fun x -> if x % 2 = 0 then 1 else 0) |> List.sum

let countEvenFilter list =
    list |> List.filter (fun x -> x % 2 = 0) |> List.length

let countEvenFold list =
    list |> List.fold (fun acc x -> if x % 2 = 0 then acc + 1 else acc) 0
