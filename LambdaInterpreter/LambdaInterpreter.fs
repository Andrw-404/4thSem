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
        getNewName (name + "'") usedName
    else
        name

let rec substitute nameToFind replacement target =
    match target with
    | Variable foundName when foundName = nameToFind -> replacement
    | Variable _ -> target
    | Application (left, right) ->
        Application(substitute nameToFind replacement left, substitute nameToFind replacement right)
    | LambdaAbstraction (argumentName, _) when argumentName = nameToFind -> target
    | LambdaAbstraction (argumentName, body) ->
        let fvBody = findFV body

        if not (Set.contains nameToFind fvBody) then
            target
        else
        let fvReplacement = findFV replacement

        if Set.contains argumentName fvReplacement then
            let forbiddenNames = Set.union fvReplacement fvBody
            let newName = getNewName argumentName forbiddenNames

            let alphaConvertBody = substitute argumentName (Variable newName) body

            LambdaAbstraction(newName, substitute nameToFind replacement alphaConvertBody)
        else
            LambdaAbstraction(argumentName, substitute nameToFind replacement body)

let rec doOneStepReduction term =
    match term with
    | Application (LambdaAbstraction (argumentName, body), argument) -> Some(substitute argumentName argument body)
    | Application (left, right) ->
        match doOneStepReduction left with
        | Some newLeft -> Some(Application(newLeft, right))
        | None ->
            match doOneStepReduction right with
            | Some newRight -> Some(Application(left, newRight))
            | None -> None
    | LambdaAbstraction (argumentName, body) ->
        match doOneStepReduction body with
        | Some newBody -> Some(LambdaAbstraction(argumentName, newBody))
        | None -> None
    | Variable _ -> None

let rec evaluate maxSteps term =
    if maxSteps <= 0 then
        term
    else
        match doOneStepReduction term with
        | Some nextStepTerm -> evaluate (maxSteps - 1) nextStepTerm
        | None -> term
