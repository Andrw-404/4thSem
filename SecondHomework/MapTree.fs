module MapTreeTask

type Tree<'a> =
    | Node of 'a * Tree<'a> * Tree<'a>
    | Empty

let rec mapTree func tree =
    match tree with
    | Empty -> Empty
    | Node(value, left, right) ->
        let newValue = func value
        let newLeft = mapTree func left
        let newRight = mapTree func right
        Node(newValue, newLeft, newRight)
