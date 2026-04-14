module MultiThreadedTests

open System.Threading
open System.Threading.Tasks
open NUnit.Framework
open FsUnit
open MyLazy

[<Test>]
let ``LazyMultiThreaded корректно вычисляет значение один раз`` () =
    let callCount = ref 0

    let supplier () =
        Interlocked.Increment(callCount) |> ignore
        Thread.Sleep(50)
        100

    let lazyInt = LazyMultiThreaded(supplier) :> ILazy<int>

    let tasks = Array.init 50 (fun _ -> Task.Run(fun () -> lazyInt.Get()))

    let results = Task.WhenAll(tasks).Result

    for res in results do
        res |> should equal 100

    !callCount |> should equal 1

[<Test>]
let ``LazyLockFree возвращает один и тот же объект`` () =
    let callCount = ref 0

    let supplier () =
        Interlocked.Increment(callCount) |> ignore
        Thread.Sleep(50)
        obj ()

    let lazyObj = LazyLockFree(supplier) :> ILazy<obj>

    let tasks = Array.init 50 (fun _ -> Task.Run(fun () -> lazyObj.Get()))

    let results = Task.WhenAll(tasks).Result

    let firstResult = results.[0]

    for res in results do
        Assert.AreSame(firstResult, res)

    !callCount |> should be (greaterThan 0)
