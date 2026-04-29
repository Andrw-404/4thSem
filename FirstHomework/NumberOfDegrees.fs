module NumberOfDegrees

let generatePowers n m =
    if m < 0 then
        Error "m не может быть отрицательным"
    else
        let startValue = 2.0 ** float n

        let rec helper acc =
            function
            | 0 -> List.rev acc
            | remaining ->
                let nextValue = List.head acc * 2.0
                helper (nextValue :: acc) (remaining - 1)

        Ok(helper [ startValue ] m)