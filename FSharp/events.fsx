#r "nuget: Spectre.Console" ; open Spectre.Console


// new event
let e = Event<string>()

let stream = e.Publish

//observable
stream
|> Observable.subscribe (fun s -> printfn "My Name is %s" s)

["Jeff"; "Hugh"; "Barry"]
|> List.iter (fun n -> e.Trigger n)

//can't dynamically add or update rows to the table without rerendering
//https://github.com/spectresystems/spectre.console/issues/156
let simple = 
    let t = Table()
             .Centered()
             .BorderColor(Color.Red)
             .BorderColor(Color.Red)
             .AddColumn(TableColumn("[u]ABC[/]"))
             .AddColumn(TableColumn("[u]XYZ[/]"))
             .AddColumn(TableColumn("[u]PQR[/]"))
             .AddRow("Hello", "[red]World![/]", "")
             .AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]")
             .AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "")
    t
     //.AddColumn(new TableColumn("[u]CDE[/]").Footer("EDC").Centered())
                // .AddColumn(new TableColumn("[u]FED[/]").Footer("DEF"))
                // .AddColumn(new TableColumn("[u]IHG[/]").Footer("GHI"))
                // .AddRow("Hello", "[red]World![/]", "")
                // .AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]")
                // .AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "")

AnsiConsole.Render(simple)
