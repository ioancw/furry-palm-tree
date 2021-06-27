//cards
type Suit = Diamonds | Hearts | Clubs | Spades 

type Face = 
    | Two | Three | Four | Five | Six | Seven | Eight | Nine | Ten 
    | Jack | Queen | King | Ace 

type Card = {Face: Face; Suit: Suit}

let allFaces = 
    [
        Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten;
        Jack; Queen; King; Ace
    ]

let allSuits = [Diamonds; Hearts; Clubs; Spades]
//applicative lists
let (<*>) fs l = fs |> List.collect (fun f -> l |> List.map f)

let funky = fun f s -> {Face = f; Suit = s}

let fullDeck = [funky] <*> allFaces <*> allSuits
fullDeck |> List.take 6 |> List.iter (printfn "%A")

//not really the most idiomatic way to do this in F#
let fullDesk' = 
    [
        for suit in allSuits do
        for face in allFaces do
        {Face = face; Suit = suit}
    ]

//another use of applicative functor in a list
//function which takes 7 strings and returns a string
let funky' = [sprintf "%s%s%s%s%s%s"]

funky' <*> ["P";"p"] <*> ["a";"4"] <*> ["ssw"] <*> ["o"; "O"] <*> ["rd"] <*> ["";"!"]

//get a int from a string option and return as int option

//two lists lookup
let first = 
    [
        "A", "XYZ"
        "B", "PQR"
        "C", "LMN"
        "D", "DEF"
    ]
    |> Map.ofList

let second = 
    [
        "DEF", 1.0
        "XYZ", 2.0
    ]
    |> Map.ofList

let find something = 
    match first.TryFind something with 
    | Some r -> 
        match second.TryFind r with 
        | Some v -> Some v
        | _ -> None 
    | _ -> None

find "B"

let find2 something = 
    something 
    |> first.TryFind 
    |> Option.bind second.TryFind

find2 "D"

let testVal = 1.5 
let testString = testVal.ToString() |> Some 

//if valid convert to int option
open System
match testString with 
| Some s ->
    match Double.TryParse s with 
    | true, v -> Some v
    | _ -> None
| _ -> None

let parseDouble (s: string) = 
    match Double.TryParse s with 
    | true, v -> Some v 
    | _ -> None

testString |> Option.bind parseDouble //for when the function already returns an option
testString |> Option.map parseDouble //for when the function returns a non option, which will be wrapped in an option.

//if you need to use connect, it's likely that you need to use Option.bind instead of Option.map