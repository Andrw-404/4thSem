module LambdaParser

open System
open FParsec
open LambdaInterpreter

type LambdaProgram = (string * term) list * term

let pterm, ptermRef = createParserForwardedToRef<term, unit> ()
let spacesNoNewline = skipMany (pchar ' ')
let spacesNoNewline1 = skipMany1 (pchar ' ')
let pnewline = pchar '\n' |>> ignore
let pblankLines = skipMany (spacesNoNewline >>. pnewline)
let (!) parser = parser .>> spacesNoNewline

let pidentifier =
    let isIdFirst symbol = isAsciiLetter symbol

    let isIdRest symbol =
        isAsciiLetter symbol
        || isDigit symbol
        || symbol = '_'

    many1Satisfy2L isIdFirst isIdRest "identifier"
    >>= fun id ->
            if id = "let" then
                fail "let нельзя использовать как имя переменной"
            else
                preturn id

let punit =
    (!pidentifier |>> Variable)
    <|> (!(pstring "(") >>. pterm .>> !(pstring ")"))

let punit_sequence =
    many1 punit
    |>> List.reduce (fun acc x -> Application(acc, x))

let pid_list = many1 !pidentifier

let plambda_expr =
    !(pstring "\\") >>. pid_list .>> !(pstring ".")
    .>>. pterm
    |>> fun (args, body) -> List.foldBack (fun arg acc -> LambdaAbstraction(arg, acc)) args body

ptermRef.Value <- plambda_expr <|> punit_sequence

let plet_binding =
    pstring "let"
    >>. spacesNoNewline1
    >>. !pidentifier
    .>> !(pstring "=")
    .>>. pterm
    .>> spacesNoNewline
    .>> pnewline

let pprogram =
    spacesNoNewline
    >>. pblankLines
    >>. many (plet_binding .>> pblankLines)
    .>>. pterm
    .>> spacesNoNewline
    .>> pblankLines
    .>> eof

let rec termToString =
    function
    | Variable x -> x
    | Application (leftTerm, rightTerm) ->
        let leftStr =
            match leftTerm with
            | LambdaAbstraction _ -> sprintf "(%s)" (termToString leftTerm)
            | _ -> termToString leftTerm

        let rightStr =
            match rightTerm with
            | Application _ -> sprintf "(%s)" (termToString rightTerm)
            | _ -> termToString rightTerm

        sprintf "%s %s" leftStr rightStr
    | LambdaAbstraction (x, body) -> sprintf "\\%s.%s" x (termToString body)

let evaluateProgram maxSteps ((bindings, mainTerm): LambdaProgram) =
    let fullTerm =
        List.foldBack (fun (name, expr) acc -> Application(LambdaAbstraction(name, acc), expr)) bindings mainTerm

    LambdaInterpreter.evaluate maxSteps fullTerm

let runString input =
    match run pprogram input with
    | Success (program, _, _) ->
        let reducedTerm = evaluateProgram 1000 program
        termToString reducedTerm
    | Failure (errorMsg, _, _) -> sprintf "Ошибка парсинга: %s" errorMsg

let runFile path =
    match runParserOnFile pprogram () path Text.Encoding.UTF8 with
    | Success (program, _, _) ->
        let reducedTerm = evaluateProgram 1000 program
        termToString reducedTerm
    | Failure (errorMsg, _, _) -> sprintf "Ошибка парсинга файла: %s" errorMsg