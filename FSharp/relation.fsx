let columns = [ "A"; "B"; "C" ]

let data =
    [ [ "one"; "two"; "three" ]
      [ "one"; "two"; "three" ]
      [ "four"; "five"; "six" ]
      [ "seven"; "eight"; "nine" ] ]

type Relation =
    { columns: string list
      data: string list list }

let addToSet data = 
    data |> Set.ofList

columns |> Set.ofList