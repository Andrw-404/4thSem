module FirstHomework.NumberofDegreesTests

open NUnit.Framework
open FsUnit
open NumberOfDegrees

[<Test>]
let ``При m<0 возвращается пустой список`` () =
    generatePowers 5 -1 |> should be Empty

[<Test>]
let ``При m=0 возвращается список с единственным элементом 2^n `` () =
    generatePowers 5 0 |> should equal [32]

[<Test>]
let ``Генерация степеней, начиная с n=0`` () =
    generatePowers 0 5 |> should equal [1;2;4;8;16;32]

[<Test>]
let ``Генерация степеней, начиная с ненулевого n`` () =
    generatePowers 3 3 |> should equal [8;16;32;64]