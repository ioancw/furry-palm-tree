open System.IO

let fileName = "d1_1.txt"

let data =
    Path.Combine(__SOURCE_DIRECTORY__, fileName)
    |> File.ReadAllLines
    |> List.ofArray

let collect x = 
    function
    | xs :: xss when x <> "" -> (x::xs) :: xss
    | xss -> if x <> "" then [ x ] :: xss else [] :: xss

let summedData =
    List.foldBack collect data []
    |> List.map (List.sumBy int)

let part1 =
    summedData
    |> List.max

let part2 =
    summedData
    |> List.take 3
    |> List.sum





