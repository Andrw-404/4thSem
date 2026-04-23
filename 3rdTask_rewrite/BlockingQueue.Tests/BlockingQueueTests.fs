module BlockingQueueTests

open NUnit.Framework
open FsUnit
open BlockingQueue
open System.Threading
open System

[<Test>]
let ``BlockingQueue извлечение происходит в корректном порядке`` () =
    let testQueue = BlockingQueue<int>()
    testQueue.Enqueue(1)
    testQueue.Enqueue(2)
    testQueue.Enqueue(3)

    testQueue.Dequeue() |> should equal 1
    testQueue.Dequeue() |> should equal 2
    testQueue.Dequeue() |> should equal 3

[<Test>]
let ``BlockingQueue добавление и извлечение элемента работает корректно`` () =
    let testQueue = BlockingQueue<int>()
    testQueue.Enqueue(2)
    let result = testQueue.Dequeue()
    result |> should equal 2

[<Test>]
let ``BlockingQueue корректно работает со строками`` () =
    let testQueue = BlockingQueue<string>()
    testQueue.Enqueue("1")
    testQueue.Enqueue("2")
    testQueue.Dequeue() |> should equal "1"
    testQueue.Dequeue() |> should equal "2"

[<Test>]
let ``BlockingQueue извлечение блокирует поток до появления элемента`` () =
    let testQueue = BlockingQueue<int>()
    let mutable dequeueValue = 0
    let syncEvent = new ManualResetEvent(false)

    let consumer =
        Thread (fun () ->
            dequeueValue <- testQueue.Dequeue()
            syncEvent.Set() |> ignore)

    consumer.Start()
    Thread.Sleep(65)
    consumer.IsAlive |> should be True

    testQueue.Enqueue(5)
    let completed = syncEvent.WaitOne(TimeSpan.FromSeconds(3.0))
    completed |> should be True
    dequeueValue |> should equal 5

[<Test>]
let ``BlockingQueue каждый из заблокированных потоков получает свой элемент`` () =
    let testQueue = BlockingQueue<int>()
    let results = System.Collections.Concurrent.ConcurrentBag<int>()

    let consumers =
        Array.init 3 (fun _ -> Thread(fun () -> results.Add(testQueue.Dequeue())))

    consumers |> Array.iter (fun t -> t.Start())
    Thread.Sleep(65)

    consumers
    |> Array.iter (fun t -> t.IsAlive |> should be True)

    [ 1; 2; 3 ] |> List.iter testQueue.Enqueue

    consumers
    |> Array.iter (fun t -> t.Join(1500) |> should be True)

    results.Count |> should equal 3

    results
    |> Seq.sort
    |> Seq.toList
    |> should equal [ 1; 2; 3 ]
