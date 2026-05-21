module Crawler.MockTests

open NUnit.Framework
open FsUnit
open WireMock.Server
open WireMock.RequestBuilders
open WireMock.ResponseBuilders

[<Test>]
let ``analyze должен скачивать страницы и корректно считать их размер`` () =
    use server = WireMockServer.Start()
    let baseUrl = server.Urls.[0]

    let startHtml =
        sprintf """<html><body><a href="%s/child1">first</a></body></html>""" baseUrl

    let childHtml = "12345678910111213"

    server
        .Given(Request.Create().WithPath("/start").UsingGet())
        .RespondWith(Response.Create().WithStatusCode(200).WithBody(startHtml))
    |> ignore

    server
        .Given(Request.Create().WithPath("/child1").UsingGet())
        .RespondWith(Response.Create().WithStatusCode(200).WithBody(childHtml))
    |> ignore

    let testUrl = baseUrl + "/start"
    let result = Crawler.analyze testUrl |> Async.RunSynchronously

    match result with
    | None -> Assert.Fail("analyze вернул None, когда должна быть успешно загружена страница")

    | Some(startSize, childResults) ->
        startSize |> should equal startHtml.Length

        childResults
        |> should contain (baseUrl + "/child1", Some childHtml.Length)

[<Test>]
let ``analyze должен возвращать None, если стартовая страница отвечает ошибку`` () =
    use server = WireMockServer.Start()
    let baseUrl = server.Urls.[0]

    server.Given(Request.Create().WithPath("/start404").UsingGet()).RespondWith(Response.Create().WithNotFound())
    |> ignore

    let testUrl = baseUrl + "/start404"
    let result = Crawler.analyze testUrl |> Async.RunSynchronously
    result |> should equal None

[<Test>]
let ``analyze должен обрабатывать несколько ссылок и возвращать None для упавших дочерних ссылок`` () =
    use server = WireMockServer.Start()
    let baseUrl = server.Urls.[0]

    let startHtml =
        sprintf """<html><body>
            <a href="%s/child1">first</a>
            <a href="%s/child2">second</a>
            <a href="%s/errorChild">third</a>
            </body></html>""" baseUrl baseUrl baseUrl

    let firstChildHtml = "Hello world from first child"
    let secondChildHtml = "Hello world from second child12345"

    server
        .Given(Request.Create().WithPath("/start").UsingGet())
        .RespondWith(Response.Create().WithStatusCode(200).WithBody(startHtml))
    |> ignore

    server
        .Given(Request.Create().WithPath("/child1").UsingGet())
        .RespondWith(Response.Create().WithStatusCode(200).WithBody(firstChildHtml))
    |> ignore

    server
        .Given(Request.Create().WithPath("/child2").UsingGet())
        .RespondWith(Response.Create().WithStatusCode(200).WithBody(secondChildHtml))
    |> ignore

    server
        .Given(Request.Create().WithPath("/errorChild").UsingGet())
        .RespondWith(Response.Create().WithStatusCode(600))
    |> ignore

    let testUrl = baseUrl + "/start"
    let result = Crawler.analyze testUrl |> Async.RunSynchronously

    match result with
    | None -> Assert.Fail("analyze вернул None, хотя стартовая страница была успешно загружена")
    | Some(startSize, childResults) ->
        startSize |> should equal startHtml.Length
        childResults |> should haveLength 3
        childResults |> should contain (baseUrl + "/child1", Some firstChildHtml.Length)
        childResults |> should contain (baseUrl + "/child2", Some secondChildHtml.Length)
        childResults |> should contain (baseUrl + "/errorChild", None)