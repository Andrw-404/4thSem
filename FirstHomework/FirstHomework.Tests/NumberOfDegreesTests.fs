module FirstHomework.NumberofDegreesTests

open NUnit.Framework
open FsUnit
open NumberOfDegrees

let ok (value: float list) : Result<float list, string> = Ok value
[<Test>]
let ``При m<0 возвращается пустой список`` () =
    generatePowers 5 -1 |> should equal (Error "m не может быть отрицательным" : Result<float list, _>)

[<Test>]
let ``При m=0 возвращается список с единственным элементом 2^n `` () =
    generatePowers 5 0 |> should equal (ok [32.0])

[<Test>]
let ``Генерация степеней, начиная с n=0`` () =
    generatePowers 0 5 |> should equal (ok [1.0;2.0;4.0;8.0;16.0;32.0])

[<Test>]
let ``Генерация степеней, начиная с ненулевого n`` () =
    generatePowers 3 3 |> should equal (ok [8.0;16.0;32.0;64.0])

[<Test>]
let ``Генерация степеней с отрицательным n`` () =
    generatePowers -2 2 |> should equal (ok [0.25;0.5;1.0])