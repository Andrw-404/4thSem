module SecondHomework.MapTreeTests

open NUnit.Framework
open MapTreeTask
open FsUnit

[<Test>]
let ``mapTree корректно увеличивает значение в дереве`` () =
    let tree = Node(4, Node(2, Tree.Empty, Tree.Empty), Tree.Empty)
    let expected = Node(8, Node(4, Tree.Empty, Tree.Empty), Tree.Empty)

    mapTree (fun x -> x * 2) tree |> should equal expected
