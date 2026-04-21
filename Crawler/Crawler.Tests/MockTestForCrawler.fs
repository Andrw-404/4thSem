module Crawler.MockTests

open NUnit.Framework
open FsUnit
open WireMock.Server
open WireMock.RequestBuilders
open WireMock.ResponseBuilders

[<Test>]
let ``analyze должен скачивать страницы и корректно считать их размер`` () =
    use server = WireMockServer.Start(8080)

    let startHtml =
        """<html><body><a href="http://localhost:8080/child1">first</a></body></html>"""

    server
        .Given(Request.Create().WithPath("/start").UsingGet())
        .RespondWith(
            Response
                .Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "text/html")
                .WithBody(startHtml)
        )
    |> ignore

    let childHtml = "12345678910111213"

    server
        .Given(Request.Create().WithPath("/child1").UsingGet())
        .RespondWith(
            Response
                .Create()
                .WithStatusCode(200)
                .WithBody(childHtml)
        )
    |> ignore

    let testUrl = "http://localhost:8080/start"
    let result = Crawler.analyze testUrl |> Async.RunSynchronously

    match result with
    | None -> Assert.Fail("analyze returned None, when should successful downloading page")

    | Some (startSize, childResults) ->
        startSize |> should equal startHtml.Length

        childResults |> should haveLength 1

        childResults
        |> should contain ("http://localhost:8080/child1", Some childHtml.Length)
