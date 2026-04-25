module NumberofDegrees

let generatePowers n m = 
    if m<0 then []
    else
        let rec loop count currentValue acc = 
            if count > m then
                List.rev acc
            else
                loop (count + 1) (currentValue * 2) (currentValue :: acc)

        let startValue = pown 2 n
        loop 0 startValue []