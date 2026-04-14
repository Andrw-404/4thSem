module MyLazy

open System.Threading

type ILazy<'a> =
    abstract member Get: unit -> 'a

type SingleThreadedLazy<'a>(supplier: unit -> 'a) =
    let mutable isEvaluated = false
    let mutable result = Unchecked.defaultof<'a>
    let mutable supplierRef = supplier

    interface ILazy<'a> with
        member this.Get() =
            if not isEvaluated then
                result <- supplierRef ()
                isEvaluated <- true
                supplierRef <- Unchecked.defaultof<_>

            result

type LazyMultiThreaded<'a>(supplier: unit -> 'a) =
    let syncObj = obj ()
    let mutable isEvaluated = false
    let mutable result = Unchecked.defaultof<'a>
    let mutable supplierRef = supplier

    interface ILazy<'a> with
        member this.Get() =
            if not (Volatile.Read(&isEvaluated)) then
                lock syncObj (fun () ->
                    if not (Volatile.Read(&isEvaluated)) then
                        result <- supplierRef ()
                        Volatile.Write(&isEvaluated, true)
                        supplierRef <- Unchecked.defaultof<_>)

            result

[<AllowNullLiteral>]
type ResultBox<'a>(value: 'a) =
    member val Value = value

type LazyLockFree<'a>(supplier: unit -> 'a) =
    let mutable resultBox = Unchecked.defaultof<ResultBox<'a>>
    let mutable supplierRef = supplier

    interface ILazy<'a> with
        member this.Get() =
            let currentBox = Volatile.Read(&resultBox)

            if not (isNull (box currentBox)) then
                currentBox.Value
            else
                let s = supplierRef

                let evaluatedValue =
                    if isNull (box s) then
                        Unchecked.defaultof<'a>
                    else
                        s ()

                let newBox = ResultBox(evaluatedValue)
                let oldBox = Interlocked.CompareExchange(&resultBox, newBox, null)

                if isNull (box oldBox) then
                    supplierRef <- Unchecked.defaultof<_>
                    evaluatedValue
                else
                    oldBox.Value
