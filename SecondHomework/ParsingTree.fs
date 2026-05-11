module ParsingTreeTask

type Expr =
    | Value of int
    | Add of Expr * Expr
    | Sub of Expr * Expr
    | Mul of Expr * Expr
    | Div of Expr * Expr

let rec calculate expr =
    match expr with
    | Value x -> x
    | Add(a, b) -> calculate a + calculate b
    | Sub(a, b) -> calculate a - calculate b
    | Mul(a, b) -> calculate a * calculate b
    | Div(a, b) -> calculate a / calculate b
