open System.IO

let fileName = "d4_1.txt"

let data =
    Path.Combine(__SOURCE_DIRECTORY__, fileName)
    |> File.ReadAllLines
    |> List.ofArray

let testData =
    [
        "2-4,6-8"
        "2-3,4-5"
        "5-7,7-9"
        "2-8,3-7"
        "6-6,4-6"
        "2-6,4-8"
    ]

let splitPairs (input: string) =
    let inputSplit = input.Split([|','|])
    inputSplit[0], inputSplit[1]
    
let splitPairs2 (input:string) =
    match input.Split(",") with
    | [| first; second |] -> first, second
    | _ -> failwith "Invalid pair"
    
let createSets (input: string) =
    let inputSplit = input.Split([|'-'|])
    [int inputSplit[0] .. int inputSplit[1] ]
    |> Set.ofList

let assignments f = splitPairs >> f >> createSets

let parse (input: string) =
    let firstAssignments = input |> assignments fst 
    let secondAssignment = input |> assignments snd
    firstAssignments, secondAssignment
    
let fullyContained (first, second) =
    Set.isSubset first second || Set.isSubset second first 

// part1    
data
|> List.map parse
|> List.map fullyContained
|> List.filter (fun i -> i = true)
|> List.length

// part 2

let overlap (first, second) =
    Set.intersect first second
    |> Set.isEmpty
    
data
|> List.map parse
|> List.map overlap
|> List.filter (fun i -> i = false)
|> List.length    
    
    
    