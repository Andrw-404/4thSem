module SingleThreadedTests

open MyLazy
open NUnit.Framework
open FsUnit

[<Test>]
let ``Get корректно возвращает значение`` () =
    let lazyValue = SingleThreadedLazy(fun () -> 45) :> ILazy<int>
    lazyValue.Get() |> should equal 45

[<Test>]
let ``Get вычиляет значение один раз`` () =
    let callCount = ref 0

    let supplier () =
        callCount := !callCount + 1
        45

    let lazyValue = SingleThreadedLazy(supplier) :> ILazy<int>

    lazyValue.Get() |> should equal 45
    lazyValue.Get() |> should equal 45

    let tmp = !callCount
    tmp |> should equal 1

[<Test>]
let ``Get корректно обрабатывает null`` () =
    let lazyValue = SingleThreadedLazy(fun () -> null) :> ILazy<string>
    lazyValue.Get() |> should be Null
