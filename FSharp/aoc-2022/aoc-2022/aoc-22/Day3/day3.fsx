open System.IO

let fileName = "d3_1.txt"

let data =
    Path.Combine(__SOURCE_DIRECTORY__, fileName)
    |> File.ReadAllLines
    |> List.ofArray
    
let testFile =
    [
        "vJrwpWtwJgWrhcsFMMfFFhFp"
        "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL"
        "PmmdzqPrVvPwwTWBwg"
        "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn"
        "ttgJtRGJQctTZtZT"
        "CrZsJsPPZsGzwwsLwLmpwMDw"
    ]
    
let getPriorities letter =     
    let lowPriorities = List.zip ['a' .. 'z'] [1 .. 26]
    let highPriorities = List.zip ['A' .. 'Z'] [27 .. 52]
    let map = lowPriorities @ highPriorities |> Map.ofList
    map[letter]

let processSets input =
    input
    |> Seq.map Set.ofSeq
    |> Set.intersectMany
    |> Set.map getPriorities
    
let calculatePriorities (data: string list) =
    data
    |> List.map (Seq.splitInto 2 >> processSets)
    |> Seq.sumBy Set.maxElement
    
calculatePriorities testFile
calculatePriorities data

// part 2
let calculateBadges (data: string list) = 
    data
    |> List.chunkBySize 3
    |> List.map processSets
    |> Seq.sumBy Set.maxElement
    
calculateBadges testFile
calculateBadges data


