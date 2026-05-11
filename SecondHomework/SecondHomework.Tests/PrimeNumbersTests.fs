module SecondHomework.PrimeNumbersTests

open NUnit.Framework
open PrimeNumbersTask
open FsUnit

[<Test>]
let ``Число 5 является простым`` () = isPrime 5 |> should be True

[<Test>]
let ``Числа 0 и 1 не являются простыми`` () =
    isPrime 0 |> should be False
    isPrime 1 |> should be False

[<Test>]
let ``Первые 5 простых чисел корерктно генерируются`` () =
    let expected = [ 2; 3; 5; 7; 11 ]
    let test = getPrimesNum |> Seq.take 5 |> Seq.toList
    test |> should equal expected
