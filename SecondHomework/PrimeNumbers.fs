module PrimeNumbersTask

let isPrime n =
    if n < 2 then
        false
    else
        let sqrt = int (sqrt (float n))

        seq { 2..sqrt } |> Seq.forall (fun i -> n % i <> 0)

let getPrimesNum = Seq.initInfinite (fun i -> i + 2) |> Seq.filter isPrime
