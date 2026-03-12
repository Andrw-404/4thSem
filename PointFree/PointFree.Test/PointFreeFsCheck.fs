module PointFree.Test

open NUnit.Framework
open FsCheck

[<Test>]
let ``Point free версия должна вести себя как оригинал`` () =
    let func0 x l = List.map (fun y -> y * x) l
    let func6: int -> int list -> int list = List.map << (*)
    let checker (x: int) (l: int list) = func0 x l = func6 x l
    Check.QuickThrowOnFailure checker
