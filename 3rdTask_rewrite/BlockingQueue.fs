/// module for Blocking Queue
module BlockingQueue

open System.Threading
open System.Collections.Generic

/// <summary>
/// A thread-safe blocking queue implementation.
/// </summary>
type BlockingQueue<'T>() =
    let queue = Queue<'T>()
    let locker = obj ()

    /// <summary>
    /// Adds an item to the end of the queue and wakes up a waiting thread.
    /// </summary>
    /// <param name="item">The item to add to the queue.</param>
    member this.Enqueue(item: 'T) =
        lock locker (fun () ->
            queue.Enqueue(item)
            Monitor.Pulse(locker))

    /// <summary>
    /// Removes and returns the object at the beginning of the queue.
    /// Blocks the calling thread if the queue is empty until an item is available.
    /// </summary>
    /// <returns>The item removed from the queue.</returns>
    member this.Dequeue() : 'T =
        lock locker (fun () ->
            while queue.Count = 0 do
                Monitor.Wait(locker) |> ignore

            queue.Dequeue())
