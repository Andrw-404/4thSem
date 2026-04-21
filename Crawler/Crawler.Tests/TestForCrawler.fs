module Crawler.Tests

open FsUnit
open NUnit.Framework

[<Test>]
let ``findChildernAddress должен обрабатывать http и https ссылки`` () =
    let html =
        """<p>123 <a href="https://abcde.com">gfdsa</a></p>
        <p>fdvds<a href="http://dsak.com">second</a></p>"""

    let result = Crawler.findChildAddress html
    result |> should haveLength 2

    result
    |> should
        equal
        [ "https://abcde.com"
          "http://dsak.com" ]

[<Test>]
let ``findChildernAddress должен игнорировать ссылки, не удовлетворяющие шаблону`` () =
    let html =
        """<p>123 <a href="/abcde.com">gfdsa</a></p>
        <p>fdvds<a href="//searchdsak.com">second</a></p>"""

    let result = Crawler.findChildAddress html
    result |> should be Empty

[<Test>]
let ``findChildernAddress игнорирует дубликаты`` () =
    let html =
        """<p>123 <a href="https://abcde.com">gfdsa</a></p>
        <p>123 <a href="https://abcde.com">gfdsa</a></p>"""

    let result = Crawler.findChildAddress html
    result |> should haveLength 1
    result |> should equal [ "https://abcde.com" ]

[<Test>]
let ``findChildernAddress возвращает пустой список для пустого или не содержащего ссылок html`` () =
    let emptyHtml = ""

    Crawler.findChildAddress emptyHtml
    |> should be Empty

    let noLinksHtml = "<h1>12345abcdef</h1>"

    Crawler.findChildAddress noLinksHtml
    |> should be Empty
