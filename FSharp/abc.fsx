#r "nuget: Spectre.Console"

open Spectre.Console

AnsiConsole.Foreground <- Color.CornflowerBlue
AnsiConsole.WriteLine "Hello"

let table = Table()
table.Border <- TableBorder.Rounded
table.AddColumns (TableColumn "[u]Name[/]")
AnsiConsole.Render table