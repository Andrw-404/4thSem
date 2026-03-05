module LambdaInterpreter

type term =
    | Variable of string
    | Application of term * term
    | LambdaAbstraction of string * term

let rec findFV term =
    match term with
    | Variable x -> Set.singleton x
    | Application (a, b) -> Set.union (findFV a) (findFV b)
    | LambdaAbstraction (x, b) -> Set.remove x (findFV b)

let rec getNewName name usedName =
    if Set.contains name usedName then
        getNewName (name + "1") usedName
    else
        name

let rec substitute nameToFind replacement target =
    match target with
    | Variable foundName when foundName = nameToFind -> replacement
    | Variable foundName -> Variable foundName
    | Application (left, right) ->
        Application(substitute nameToFind replacement left, substitute nameToFind replacement right)
    | LambdaAbstraction (argumentName, body) when argumentName = nameToFind -> LambdaAbstraction(argumentName, body)
    | LambdaAbstraction (argumentName, body) ->
        let fvReplacement = findFV replacement

        if Set.contains argumentName fvReplacement then
            let fvBody = findFV body
            let forbiddenNames = Set.union fvReplacement fvBody
            let newName = getNewName argumentName forbiddenNames

            let alphaConvertBody = substitute argumentName (Variable newName) body

            LambdaAbstraction(newName, substitute nameToFind replacement alphaConvertBody)
        else
            LambdaAbstraction(argumentName, substitute nameToFind replacement body)
