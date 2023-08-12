////////////////////21 card trick

//get 21 cards from deck
//choose cards

//deal into three piles.
//point at pile containing cards
//collect cards with pile containing card in the middle.

//repeat twice.
//unknown card is 11th card.

open System

let shuffle (r: Random) xs = xs |> Seq.sortBy (fun _ -> r.Next())

let cards21 deck =
    deck |> shuffle (Random()) |> Seq.take 21 |> Seq.toList

let pile n =
    seq { (n - 1) .. 3 .. 21 - 1 } |> Seq.toList

let getPiles (deck: (string * string) list) =
    let p1 =
        [ for x in pile 1 do
              yield deck.[x] ]

    let p2 =
        [ for x in pile 2 do
              yield deck.[x] ]

    let p3 =
        [ for x in pile 3 do
              yield deck.[x] ]

    (p1, p2, p3)

let printPiles (p1: (string * string) list) (p2: (string * string) list) (p3: (string * string) list) =
    for i in [ 0..6 ] do
        printfn
            "%s of %s : %s of %s : %s of %s"
            (fst p1.[i])
            (snd p1.[i])
            (fst p2.[i])
            (snd p2.[i])
            (fst p3.[i])
            (snd p3.[i])
    |> ignore

let ranks =
    [ "A"; "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9"; "10"; "J"; "Q"; "K" ]

let suits = [ "\u2665"; "\u2663"; "\u2666"; "\u2660" ]

let deck =
    [ for card in ranks do
          for suit in suits do
              yield (card, suit) ]

let endGame (deck: (string * string) list) =
    printfn "Card is %s of %s" (fst deck.[10]) (snd deck.[10])

let rec simulateGame (deck: (string * string) list) =
    let p1, p2, p3 = getPiles deck
    printPiles p1 p2 p3
    Console.WriteLine "Which pile contains card?"
    let columnOfCard = Console.ReadLine()

    match columnOfCard |> int with
    | 1 -> simulateGame (p2 @ p1 @ p3)
    | 2 -> simulateGame (p1 @ p2 @ p3)
    | 3 -> simulateGame (p1 @ p3 @ p2)
    | _ -> endGame (p1 @ p2)

let runGame =
    let ranks =
        [ "A"; "1"; "2"; "3"; "4"; "5"; "6"; "7"; "8"; "9"; "10"; "J"; "Q"; "K" ]

    let suits = [ "\u2665"; "\u2663"; "\u2666"; "\u2660" ]

    let deck =
        [ for card in ranks do
              for suit in suits do
                  (card, suit) ]

    let choiceDeck = cards21 deck
    let space = Console.ReadLine()

    for card in choiceDeck do
        printfn $"%s{fst card} of %s{snd card}"

    printfn "Now choose a card.  I will guess it. Hit any key to continue."
    let space = Console.ReadLine()
    simulateGame choiceDeck

runGame

//Domain modelling of cards

type Suit =
    | Hearts
    | Clubs
    | Diamonds
    | Spades

    static member GetSuits() = [ Hearts; Diamonds; Clubs; Spades ]

type Rank =
    | Value of int
    | Ace
    | King
    | Queen
    | Jack

    static member GetAllRanks() =
        [ yield Ace; for i in 2..10 -> Value i; yield Jack; yield Queen; yield King ]

//record type to combine a Suit and a Rank
type Card = { Suit: Suit; Rank: Rank }

//list of cards containing a full deck
let fullDeck =
    [ for suit in Suit.GetSuits() do
          for rank in Rank.GetAllRanks() do
              { Suit = suit; Rank = rank } ]

//prints out a card
let printCard (c: Card) =
    let rankToString =
        match c.Rank with
        | Ace -> "A"
        | King -> "K"
        | Queen -> "Q"
        | Jack -> "J"
        | Value n -> string n

    let suitToString =
        match c.Suit with
        | Clubs -> "C"
        | Diamonds -> "D"
        | Spades -> "S"
        | Hearts -> "H"

    rankToString + " of " + suitToString

let printCards () =
    for card in fullDeck do
        printfn $"%s{printCard card}"

let shuffleFunc (r: Random) xs = xs |> Seq.sortBy (fun _ -> r.Next())

let getnRandomCards n deck =
    deck |> shuffleFunc (Random()) |> Seq.take n |> Seq.toList

let deal21cards = getnRandomCards 21 fullDeck

let printer = printCard >> printfn "%s"

deal21cards |> Seq.iter printer

let pileC n =
    seq { (n - 1) .. 3 .. 21 - 1 } |> Seq.toList

let getPilesC (deck: Card list) =
    let p1 =
        [ for x in pile 1 do
              yield deck.[x] ]

    let p2 =
        [ for x in pile 2 do
              yield deck.[x] ]

    let p3 =
        [ for x in pile 3 do
              yield deck.[x] ]

    (p1, p2, p3)

let p1C, p2C, p3C = getPilesC deal21cards

let printPilesC (p1: Card list) (p2: Card list) (p3: Card list) =
    for i in [ 0..6 ] do
        printfn $"%s{printCard p1.[i]} : %s{printCard p2.[i]} : %s{printCard p3.[i]}"
    |> ignore

printPilesC p1C p2C p3C

p1C
