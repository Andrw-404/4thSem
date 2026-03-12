module PointFree.task

let func0 x l = List.map (fun y -> y * x) l
let func1 (x: int) : (int list -> int list) = List.map (fun y -> y * x)
let func2 (x: int) : (int list -> int list) = List.map (fun y -> x * y)
let func3 (x: int) : (int list -> int list) = List.map (fun y -> (*) x y)
let func4 (x: int) : (int list -> int list) = List.map ((*) x)
let func5 (x: int) : (int list -> int list) = (List.map << (*)) x
let func6: int -> int list -> int list = List.map << (*)
