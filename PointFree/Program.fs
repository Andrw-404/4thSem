module PointFree.task

let multiplyOrig x l = List.map (fun y -> y * x) l
let multiplyEtaReduced x = List.map (fun y -> y * x)
let multiplyCommuted x = List.map (fun y -> x * y)
let multiplyPrefix x = List.map (fun y -> (*) x y)
let multiplyCurried x = List.map ((*) x)
let multiplyComposedWithX x = (List.map << (*)) x
let multiplyPointFree = List.map << (*)
