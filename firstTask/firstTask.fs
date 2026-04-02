/// Module for a function designed to find the minimum list item
module FirstTask

/// Finds the smallest item in the list.
/// <param name="list"> List of items.<param>
/// Returns None if an empty list is passed.
// Returns None if an empty list is passed.
let findMinValue list =
    match list with
    | [] -> None
    | _ -> Some(list |> List.reduce min)