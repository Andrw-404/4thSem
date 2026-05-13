module PointFree.Test

open NUnit.Framework
open FsCheck
open PointFree.task

[<Test>]
let ``Point free версия должна вести себя как оригинал`` () =
    let checker x l =
        multiplyOrig x l = multiplyPointFree x l

    Check.QuickThrowOnFailure checker
