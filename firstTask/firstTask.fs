/// Module for a function designed to find the minimum list item
module FirstTask

/// Finds the minimum item in the list.
/// <param name="list"> List of items.<param>
/// Returns None if the list is empty.
/// Returns Some with minimum value if the list is not empty.
let findMinValue list =
    match list with
    | [] -> None
    | _ -> Some(list |> List.reduce min)
