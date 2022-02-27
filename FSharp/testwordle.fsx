open System

type Result = Incorrect | CorrectLetter | CorrectPosition

type Round = {
    Guess: string;
    Result: Result seq;
}

type Game = {
    Answer: string;
    Rounds: Round seq;
}

// let loadWords path =
//     IO.File.ReadAllLines path

let loadWords path = 
    [|
        "arose"
        "unlit"
        "paper"
        "caulk"
        "tiger"
        "paint"
        "taint"
        "roger"
        "close"
        "spill"
        "maul"
    |]

let getRandomWord words =
    Array.get words (Random().Next(words.Length))

let checkGuess (answer: string) (guess: string) =
    let grader (a: char, g: char) =
        if a = g then Result.CorrectPosition else
            if answer.Contains g then Result.CorrectLetter else Result.Incorrect
    Seq.zip answer guess
    |> Seq.map grader 

let printColoured (colour: ConsoleColor) (letter: char) =
    Console.ForegroundColor <- colour
    printf "%c" letter
    Console.ResetColor()

let outputResult (letter: char, result: Result) = 
    match result with 
    | Incorrect -> printColoured ConsoleColor.Red letter
    | CorrectLetter -> printColoured ConsoleColor.Yellow letter
    | CorrectPosition -> printColoured ConsoleColor.Green letter

let outputRound (round: Round) = 
    Seq.iter outputResult (Seq.zip round.Guess round.Result)
    printfn ""

let hasWon (results: Result seq) =
    Seq.forall (fun result -> result.Equals(Result.CorrectPosition)) results

let containsAlphabetsOnly (text: string) : bool =
    text |> Seq.forall (fun c -> System.Char.IsLetter(c))

let rec input (validWords: string seq) : string = 
    printf "Input your guess: "
    let guess = System.Console.ReadLine()

    if containsAlphabetsOnly guess && Seq.contains guess validWords then guess else input validWords


let possibleAnswers = loadWords "answers.txt"

let playGame answer possibleWords state = 
    let rec play state =

        let guess = possibleWords |> input 
        let round = { Guess = guess; Result = checkGuess answer guess; }
        let newState = state @ [round]
        outputRound round |> ignore

        match hasWon round.Result with 
        | true -> newState
        | false when (List.length state >= 5) -> newState
        | false -> play newState
    play state


let result = 
    playGame 
        (getRandomWord possibleAnswers)
        (loadWords "guesses.txt" |> Seq.append possibleAnswers )
        []

printfn "Finished in: %d rounds" (Seq.length result.Rounds + 1)
printfn "Answer: %s" result.Answer
