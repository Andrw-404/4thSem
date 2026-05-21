module StringCalculateBuilder.Tests

open NUnit.Framework
open FsUnit
open StringCalculateBuilder

[<Test>]
let ``tryParseInt должен вернуть Some если строка является корректным числом`` () =
    tryParseInt "66" |> should equal (Some 66)
    tryParseInt "-66" |> should equal (Some -66)
    tryParseInt "0" |> should equal (Some 0)

[<Test>]
let ``tryParseInt должен вернуть None если строка является корректным числом`` () =
    tryParseInt "a" |> should equal None
    tryParseInt "ab" |> should equal None
    tryParseInt "" |> should equal None

[<Test>]
let ``Calculate должен вернуть Some с суммой, когда все переданные строки корректны`` () =
  let result = calculate {
    let! x = "30"
    let! y = "20"
    let! z = "50"
    return x+y+z
  } 
  result |> should equal (Some 100)

[<Test>]
let ``Calculate должен вернуть None, если строка некорректна`` () =
  let result = calculate {
    let! x = "30"
    let! y = "20"
    let! z = "b"
    return x+y+z
  } 
  result |> should equal None

[<Test>]
let ``Calculate корректно работает с одним значением`` () =
  let result = calculate {
    let! x = "30"
    return x
  } 
  result |> should equal (Some 30)

[<Test>]
let ``Calculate корректно работает со сложными выражениями`` () =
  let result = calculate {
    let! x = "30"
    let! y = "20"
    let! z = "50"
    return (x+y)/2*z
  } 
  result |> should equal (Some 1250)