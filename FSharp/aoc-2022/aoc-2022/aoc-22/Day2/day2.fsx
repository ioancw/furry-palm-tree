open System.IO

let fileName = "d2_1.txt"

let data =
    Path.Combine(__SOURCE_DIRECTORY__, fileName)
    |> File.ReadAllLines
    |> List.ofArray
    
let gameData = 
    data
    |> List.map (fun (s: string) ->
        let hands = s.Split([|' '|])
        (hands.[0], hands.[1] ))

type hand = | Rock | Paper | Scissors
    
let parseGameData (theirs, mine) =
    let parseHand hand = 
        match hand with
        | "A" | "X" -> Rock 
        | "B" | "Y" -> Paper
        | "C" | "Z" -> Scissors
        | _ -> failwithf $"Unsupported hand: %s{hand}"
    (parseHand theirs, parseHand mine)
    
let score hand =
    match hand with
    | Rock -> 1 | Paper -> 2 | Scissors -> 3
    
let scoreGame (theirs, mine) =
    match (theirs, mine) with
    | Rock, Scissors | Scissors, Paper | Paper, Rock -> 6, 0 // their winning hands
    | Scissors, Rock | Paper, Scissors | Rock, Paper -> 0, 6 // my winning hands
    | _, _ -> 3, 3
    
let calculateGameScore (theirs, mine) =
    let theirHand, myHand = score theirs, score mine
    let theirGame, myGame = scoreGame (theirs, mine)
    (theirHand + theirGame, myHand + myGame)
    
let calculateMyTotalScore (scores: (int * int) list) =
    scores
    |> List.map snd
    |> List.sum

// part 1
let scoreGameData gameData =
    gameData
    |> List.map parseGameData
    |> List.map calculateGameScore
    |> calculateMyTotalScore
    
scoreGameData gameData

// part 2

type Strategy = | Win | Lose | Draw
    
// new parser
let parseStrategyData (theirs, mine) =
    let parseStrategy hand =
        match hand with
        | "X" -> Lose
        | "Y" -> Draw
        | "Z" -> Win
        | _ -> failwithf $"Unsupported hand: %s{hand}"
    
    let parseHand hand =
        match hand with
        | "A" -> Rock
        | "B" -> Paper
        | "C" -> Scissors
        | _ -> failwithf $"Unsupported strategy hand: %s{hand}"
    (parseHand theirs, parseStrategy mine)
    
let applyStrategy (theirs, mine) =
    let myNewHand = 
        match theirs, mine with
        | Rock, Win | Scissors, Lose -> Paper
        | Paper, Win | Rock, Lose -> Scissors
        | Scissors, Win | Paper, Lose -> Rock
        | _, Draw -> theirs
    (theirs, myNewHand)
    
let scoreStrategyGame gameData =
    gameData
    |> List.map parseStrategyData
    |> List.map applyStrategy
    |> List.map calculateGameScore
    |> calculateMyTotalScore
    
let testData =
    [
        "A", "Y"
        "B", "X"
        "C", "Z"
    ]
    
scoreStrategyGame testData
scoreStrategyGame gameData
    
