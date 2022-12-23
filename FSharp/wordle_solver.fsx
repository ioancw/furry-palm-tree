open System.IO

let rec listToString l =
    match l with
    | [] -> ""
    | head :: tail -> head.ToString() + listToString tail

let sortString s =
    s |> Seq.sort |> Seq.toList |> listToString

let fileName = @"/Users/ioanwilliams/github/furry-palm-tree/words.txt"

// load given word list and ensure 5 letter words selected
let fiveLetterWords =
    fileName
    |> File.ReadAllLines
    |> Seq.filter (fun w -> w.Length = 5)

// analysis
let findMatchingFiveLetterWords word =
    let fivemap =
        fiveLetterWords
        |> Seq.groupBy sortString
        |> Seq.sortByDescending (snd >> Seq.length)
        |> Map.ofSeq
    match Map.tryFind word fivemap with
    | Some ws -> ws |> Seq.toArray
    | None -> Array.empty

// let words =
//     fiveLetterWords
//     |> Seq.collect string
//     |> Seq.groupBy id
//     |> Seq.map (fun (k, vs) -> string k, vs |> Seq.length)
//     |> Seq.sortByDescending snd
//     |> Seq.take 10
//     |> Seq.map fst
//     |> Seq.sort
//     |> Seq.fold (+) ""
//     |> findMatchingFiveLetterWords

// Array.splitAt 5 words

findMatchingFiveLetterWords ([|"i"; "l"; "t"; "n"; "d"|] |> Seq.sort |> Seq.fold (+) "")

// use this to determine words scores
let frequencies =
    fiveLetterWords
    |> Seq.collect id
    |> Seq.groupBy id
    |> Seq.map (fun (k, vs) -> k, vs |> Seq.length)
    |> Map.ofSeq

// work out word scores
let letterScores =
    fiveLetterWords
    |> Seq.map (fun word ->
        // this filters out words with multiple letter
        let multiplier = word |> Seq.groupBy id |> Seq.fold (fun state (f, s) ->  state * Seq.length s) 1
        let freq = word |> Seq.fold (fun state letter -> state + Map.find letter frequencies) 0
        word, freq / multiplier)
    |> Seq.sortByDescending snd

// use the following to find out the next most frequent word that doesn't contain
// the letters from arose.
let aroseSet = "arose" |> Set.ofSeq
letterScores
|> Seq.find (fun (word, _) ->
    let wSet = word |> Set.ofSeq
    aroseSet |> Seq.fold (fun state l -> state || Set.contains l wSet) false |> not)

// print letter scores
letterScores
|> Seq.groupBy snd
|> Seq.map (fun (k, vs) -> k, vs |> Seq.map fst)

let grp = letterScores |> Seq.iter (fun (w, f) -> printfn "%s:%A" w f)

// simulation functions

// answer mask
type answerMask = Green | Yellow | Grey

// takes an actual word and a guess and returns an answer mask
// i.e. actual is 'prose' guess is 'arose' mask is [0,2,2,2,2]
// this is a litte tricky to get right as you don't want to double count
//let getAnswerMask actual guess =

let actual = "prose"
let guess = "arose"
open System.Collections.Generic
let zipped =
    Seq.zip actual guess
    |> Seq.filter (fun (a, g) -> a <> g)
    |> Seq.groupBy id
    |> Seq.map (fun (k, vs) -> snd k, vs |> Seq.length)
    //|> Map.ofSeq

let letters = Seq.zip actual guess |> Seq.toList
let rec masker ls mask =
    match ls with
    | [] -> mask
    | (a, g) :: t when a = g -> masker t (mask @ [Green])
    | (a, g) :: t when actual.Contains(g |> string) -> masker t (Yellow :: mask)
    | h :: t -> masker t (Grey :: mask)

masker letters []

let collect x =
    function
    | (y :: xs) :: xss when x = y -> (x :: y :: xs) :: xss //add to start of existing list.
    | xss -> [ x ] :: xss //create a new list

List.foldBack collect ("aaaabbbcca" |> List.ofSeq) []
|> List.map (List.countBy id)

let getMask actual guess =
    let removeFirstInstance remove fromList =
        let rec removeFirst predicate = function
            | [] -> []
            | h :: t when predicate h -> t //terminates
            | h :: t -> h :: removeFirst predicate t
        removeFirst (fun i -> i = remove) fromList

    let rec masker2 (ls, count) mask =
        match (ls, count) with
        | [], _ -> mask
        | (a, g) :: t, cs when a = g ->
            masker2 (t, cs) (Green :: mask)
        | (a, g) :: t, cs ->
            let counts = cs |> List.filter (fun i -> i = g) |> List.length
            if Seq.contains g actual && counts > 0 then
                let newCounts = removeFirstInstance g cs
                masker2 (t, newCounts) (Yellow :: mask)
            else
                masker2 (t, cs) (Grey :: mask)

    let notMatched =
        Seq.zip actual guess
        |> Seq.filter (fun (a, g) -> a <> g)
        |> Seq.toList
        |> List.map fst

    let letters = Seq.zip actual guess |> Seq.toList
    masker2 (letters, notMatched) [] |> List.rev

let getMask2 actual guess =
    let removeFirstInstance remove fromList =
        let rec removeFirst predicate = function
            | [] -> []
            | h :: t when predicate h -> t //terminates
            | h :: t -> h :: removeFirst predicate t
        removeFirst (fun i -> i = remove) fromList

    let getCounts letters matchOn =
        letters |> List.filter (fun i -> i = matchOn) |> List.length

    let rec masker ls count mask =
        match (ls, count) with
        | [], _ -> mask
        | (a, g) :: t, cs ->
            if a = g then
                masker t cs (Green :: mask)
            else
                if Seq.contains g actual && getCounts cs g > 0 then
                    masker t (removeFirstInstance g cs) (Yellow :: mask)
                else
                    masker t cs (Grey :: mask)

    let notMatched zipped =
        zipped
        |> List.filter (fun (a, g) -> a <> g)
        |> List.map fst

    let letters = Seq.zip actual guess |> Seq.toList
    masker letters (notMatched letters) [] |> List.rev

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

let getMask3 actual guess =

    let letters = Seq.zip actual guess |> Seq.toList

    let folder ((count, mask): Map<'a,int> * answerMask list) (a, g) =
        if a = g then
            count, Green :: mask
        elif Seq.contains g actual && Counter.countOf count g > 0 then
            Counter.updateCount count g, Yellow :: mask
        else
            count, Grey :: mask

    List.fold folder (Counter.createCounter letters, []) letters |> snd |> List.rev

        //actual //guess
getMask2 "favor" "arose"
getMask2 "favor" "ratio"
getMask2 "favor" "carol"
getMask2 "favor" "vapor"
getMask2 "arose" "speed"
getMask2 "treat" "speed"

getMask3 "favor" "arose"
getMask3 "favor" "ratio"
getMask3 "favor" "carol"
getMask3 "favor" "vapor"
getMask3 "arose" "speed"
getMask3 "treat" "speed"

let rec removeFirst predicate = function
    | [] -> []
    | h :: t when predicate h -> t //terminates
    | h :: t -> h :: removeFirst predicate t

let removeFirst2 predicate list =
    let rec loop acc = function
        | [] -> acc
        | h :: t when predicate h ->
            printfn "%A" acc
            (List.rev acc) @ t //terminates
        | h :: t ->
            printfn "%A~%A#%A" h t acc
            loop (h :: acc) t
    loop [] list

removeFirst (fun i -> i = 'b') ['a';'a';'b';'b';'b';'c']
removeFirst2 (fun i -> i = 'b') ['a';'a';'b';'b';'b';'c']


