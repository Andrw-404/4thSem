/// Module for task about square.
module SquareTask

/// Generates a list of strings representing an empty square of '*' characters.
/// <param name="n">The size of the side of the square.</param>
/// Returns list of strings where each string is a row of the square.
/// Returns an empty list if n is less than or equal to 0.
let getSquare n =
    match n with
    | 1 -> [ "*" ]
    | _ when n <= 0 -> []
    | _ ->
        let upDownRow = String.replicate n "*"
        let rightAndLeftRow = "*" + String.replicate (n - 2) " " + "*"
        let middleRows = List.init (n - 2) (fun _ -> rightAndLeftRow)

        [ upDownRow ] @ middleRows @ [ upDownRow ]

/// Prints the generated square to the standard output console.
/// <param name="n">The side length of the square to be printed.</param>
let printSquare n = getSquare n |> List.iter (printfn "%s")
