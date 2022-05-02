open System

type Result = Incorrect | CorrectLetter | CorrectPosition

type Round = {
    Guess: string;
    Result: Result list;
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
        if a = g 
        then CorrectPosition 
        else 
            if answer.Contains g 
            then CorrectLetter 
            else Incorrect
    
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
    Seq.zip round.Guess round.Result
    |> Seq.iter outputResult 
    printfn ""

let hasWon results =
    results
    |> Seq.forall (fun result -> result = CorrectPosition)

let playGame answer possibleWords state = 
    let (|ValidLetters|NotValidLetters|) word = 
        if word |> Seq.forall (fun c -> System.Char.IsLetter(c)) 
        then ValidLetters 
        else NotValidLetters

    let (|ValidWord|NotValidWord|) word = 
        if Seq.contains word possibleWords then ValidWord else NotValidWord

    let (|FiveLetterWord|NotFiveLetterWord|) word = 
        if Seq.length word = 5 
        then FiveLetterWord
        else NotFiveLetterWord

    let rec input (): string = 
        printfn "Input your guess: "
        let guess = System.Console.ReadLine()

        match guess with 
        | ValidLetters & ValidWord & FiveLetterWord -> guess
        | NotValidWord -> 
            printfn "That wasn't a valid word."
            input ()
        | NotFiveLetterWord ->
            printfn "Guess must be a 5 letter word: "
            input ()
        | _ -> input ()
    
    let rec play state =

        let guess = input ()
        let round = { Guess = guess; Result = (checkGuess answer guess) |> Seq.toList; }
        let newState = state @ [round]
        outputRound round |> ignore

        match hasWon round.Result with 
        | true -> newState
        | false when (List.length state >= 5) -> newState
        | false -> play newState
    play state

let possibleAnswers = loadWords "answers.txt"
let word = (getRandomWord possibleAnswers)
let allWords = (loadWords "guesses.txt" |> Seq.append possibleAnswers )
playGame word allWords []



