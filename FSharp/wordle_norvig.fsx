open System.IO
open System

let random = Random()

type Answer = Green | Yellow | Grey

let colourToString colour =
    match colour with
    | Green -> "G"
    | Yellow -> "Y"
    | Grey -> "."

let correct = "GGGGG"

let result = [Green; Yellow; Grey; Green; Grey]

module String =
    let concat = List.fold (+) ""

module Counter =
    let createCounter items =
        items
        |> List.filter (fun (a, g) -> a <> g)
        |> List.map fst
        |> List.countBy id
        |> Map.ofList

    let countOf counter item =
        match Map.tryFind item counter with
        | Some c -> c
        | None -> 0

    let updateCount counter item =
        match Map.tryFind item counter with
        | Some c -> Map.add item (c - 1) counter
        | None -> counter

let scoreGuess actual guess =

    let letters = Seq.zip actual guess |> Seq.toList

    let folder ((count, mask): Map<'a,int> * Answer list) (a, g) =
        if a = g then
            count, Green :: mask
        elif Seq.contains g actual && Counter.countOf count g > 0 then
            Counter.updateCount count g, Yellow :: mask
        else
            count, Grey :: mask

    List.fold folder (Counter.createCounter letters, []) letters
    |> snd
    |> List.rev
    |> List.map colourToString
    |> String.concat

result |> List.map colourToString |> String.concat

let wordList = File.ReadAllLines(@"../Python/wordle-small.txt")

let check left right = left = right

scoreGuess "ANNEX" "NINNY" |> check "Y.G.."
scoreGuess "WORLD" "HELLO" |> check "...GY"
scoreGuess "PEEVE" "WHEEE" |> check "..GYG"

let consistentTargets guess score targets =
    targets |> Seq.filter (fun target -> scoreGuess target guess = score )
    
consistentTargets "NINNY" "Y.G.." wordList
|> consistentTargets "ENNUI" "YGG.."
        
type State = {Turn: int; Targets: string seq; Won: bool; Reply: string option}

let guesser reply targets = Seq.head targets

let play guesser wordList target = 
    let initialState = {Turn = 0; Targets = wordList; Won = false; Reply = None}
    
    let target =
        match target with
        | Some t -> t
        | None -> wordList |> Seq.item (random.Next(Seq.length wordList)) 
    
    let folder state turn =
        let guess = guesser state.Reply state.Targets
        let reply = scoreGuess target guess
        let targets = consistentTargets guess reply state.Targets
        let stateReply = defaultArg state.Reply ""
        do printfn "Guess %d: %s, Reply: %s:%s Remaining targets: %d: %A"
               state.Turn guess reply stateReply (Seq.length targets) targets 
        if reply = correct
        then {state with Targets = targets; Won = true; Reply = Some reply}
        else {state with Turn = turn; Targets = targets; Reply = Some reply}

    let rec loop state xs =
        match xs with
        | [] -> state
        | h :: t ->
            let newState = folder state h
            if newState.Won then newState else loop newState t
    loop initialState [1 .. 10]
    
play guesser wordList (Some "HELLO")
play guesser wordList None 

let getEveryNth n wordList = wordList |> Seq.chunkBySize n |> Seq.map Seq.head    
    
// here we guess ROAST and group the same replied for all the actuals.
// e.g. ROAST scored against BIRTH is Y...Y, but also ROAST scored against TREND is Y...Y
// so group these together    
wordList
|> getEveryNth 100
|> Seq.groupBy (fun word -> scoreGuess word "NINJA") 
|> Seq.map (fun (_, ss) -> Seq.length ss)
|> Seq.sort

let partition guess targets =
    targets
    |> Seq.groupBy (fun target -> scoreGuess target guess) 
    |> Map.ofSeq 
        
let few = wordList |> getEveryNth 100

let partitionsToCount (partitions: Map<string, seq<'b>>) =
    partitions
    |> Seq.map (fun x -> x.Value |> Seq.length) |> Seq.sort

let partitionsCount guess targets =
    partition guess targets |> partitionsToCount

partition "ROAST" few
partition "NINJA" few |> partitionsToCount

type Word = string
type Reply = string

type Tree =
    | Node of Node
    | Word of Word
and Node = {Guess: Word; Size: int; Branches: Map<string, Tree>}
    
let targets = few

// metric function takes an int seq and returns a float or int

let rec minimisingTree metric targets =
    if Seq.length targets = 1 then
        Word (Seq.head targets)
    else
        let guess = targets |> Seq.minBy (fun guess -> metric (partitionsCount guess targets))
        let branches = partition guess targets
        Node(
            { Guess = guess
              Size = Seq.length targets
              Branches =  branches |> Map.map (fun _ -> minimisingTree metric)})
        
let rec iter word node tree =
    let recurse = iter word node
    match tree with
    | Word w -> word w
    | Node n ->
        node (n.Guess, n.Size)
        n.Branches |> Map.iter (fun k -> recurse)
        
minimisingTree Seq.max few
|> iter (printfn "%s") (printfn "%A")

let rec listelements tree =
    let printNode n =
        printfn ""
        printf "%A " (n.Guess, n.Size)
        n.Branches |> Map.iter (fun k -> listelements)
    match tree with
    | Word leaf         -> printf " %s" leaf
    | Node n ->
        printNode n
        printfn ""
    
let initialTree = minimisingTree Seq.max wordList

let treeGuesser' reply targets =
    //targets |> Seq.item (random.Next(Seq.length targets)) 
    let mutable tree = initialTree
    tree <-
        match reply with
        | None -> tree
        | Some r ->
            match tree with
            | Word w -> Word w
            | Node n ->
                printfn "Reply: %s" r
                let b = n.Branches.[r]
                //printfn "*%A" b
                b
    match tree with
    | Node n -> n.Guess
    | _ -> "ERROR"

// this takes way to long ont the first set of branches.
// maybe some memoization of the reply function would help?
let onTheFlyMinimisingTree metric targets =
    if Seq.length targets = 1 then
        (Seq.head targets)
    else
        let guess = targets |> Seq.minBy (fun guess -> metric (partitionsCount guess targets))
        let newTargets = partition guess targets
        guess

let onTheFlyGuesser reply targets = 
    onTheFlyMinimisingTree Seq.max targets
    
play guesser wordList (Some "SKIRT")
play onTheFlyGuesser wordList (Some "SKIRT")    

//Now let's try to use the play function, but instead of the existing one, we change
//the state function so that it maintain the current set of tree branches.
//we pre calculate the graph and then walk the graph and store either the word or the branches left

type State2 = {Turn: int; Targets: string seq; Won: bool; Reply: string option; Nodes: Tree}
let fullTree = minimisingTree Seq.max wordList
let play2 guesser wordList target = 
    let initialState = {Turn = 0; Targets = wordList; Won = false; Reply = None; Nodes = fullTree}
    
    let target =
        match target with
        | Some t -> t
        | None -> wordList |> Seq.item (random.Next(Seq.length wordList)) 
    
    let folder state turn =
        let guess, newNode = guesser state.Reply state.Targets state.Nodes
        let reply = scoreGuess target guess
        let targets = consistentTargets guess reply state.Targets
        let stateReply = defaultArg state.Reply ""
        do printfn "Guess %d: %s, Reply: %s:%s Remaining targets: %d: %A"
               state.Turn guess reply stateReply (Seq.length targets) targets 
        if reply = correct
            then {state with Targets = targets; Won = true; Reply = Some reply; Nodes = newNode}
            else {state with Turn = turn; Targets = targets; Reply = Some reply; Nodes = newNode}

    let rec loop state xs =
        match xs with
        | [] -> state
        | h :: t ->
            let newState = folder state h
            if newState.Won then newState else loop newState t
    loop initialState [1 .. 10]

let treeGuesser reply targets node = 
    let emptyNode = {Guess = String.Empty; Size = 0; Branches = Map.empty}
    let guess, newNode = 
        match reply with 
        | None -> node.Guess, node
        | Some reply -> 
            let x = node.Branches.[reply]
            match x with 
            | Word w -> w, emptyNode 
            | Node n -> n.Guess, n
    guess, newNode
        
    
