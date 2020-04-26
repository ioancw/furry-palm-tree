#I "/Users/ioanwilliams/github/furry-palm-tree/FSharp/deedle/deedletest/bin/Debug/netcoreapp3.1"
#r "Deedle.dll"

do fsi.AddPrinter(fun (printer:Deedle.Internal.IFsiFormattable) -> "\n" + (printer.Format()))
open Deedle

let file = "/Users/ioanwilliams/github/furry-palm-tree/Julia/Other/matrix-large.csv"

let data = Frame.ReadCsv(file)

type Person = 
    {Name: string; Age: int; Countries: string list}

let peopleRecds = 
  [ { Name = "Joe"; Age = 51; Countries = [ "UK"; "US"; "UK"] }
    { Name = "Tomas"; Age = 28; Countries = [ "CZ"; "UK"; "US"; "CZ" ] }
    { Name = "Eve"; Age = 2; Countries = [ "FR" ] }
    { Name = "Suzanne"; Age = 15; Countries = [ "US" ] } ]

let peopleList = Frame.ofRecords peopleRecds
let people = peopleList |> Frame.indexRowsString "Name"

people?Age
people.GetColumn<string list>("Countries")

let peopleNested = 
    ["People" => Series.ofValues peopleRecds ] |> frame

let titanic = Frame.ReadCsv("titanic.csv")

titanic |> Frame.groupRowsByString "Sex"

titanic
|> Frame.pivotTable
    (fun k r -> r.GetAs<string>("Sex"))
    (fun k r -> r.GetAs<bool>("Survived"))
    Frame.countRows