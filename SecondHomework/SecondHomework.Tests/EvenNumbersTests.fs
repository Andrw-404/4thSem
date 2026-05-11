module SecondHomework.EvenNumbersTests

open NUnit.Framework
open EvenNumbersTask
open FsCheck

[<Test>]
let ``Функции должны возвращать одинаковый результат`` () =
    let equivalentProperty list =
        let resMap = countEvenMap list
        let resFilter = countEvenFilter list
        let resFold = countEvenFold list

        resMap = resFilter && resFilter = resFold

    Check.QuickThrowOnFailure equivalentProperty
