module WordCountTest =
    open System

    type Text = Text of string

    let addText (Text s1) (Text s2) = Text(s1 + s2)

    let wordCount (Text s) =
        System.Text.RegularExpressions.Regex.Matches(s, @"\S+").Count

    let page () =
        List.replicate 1000 "hello "
        |> List.reduce (+)
        |> Text

    let document () = page () |> List.replicate 1000

    let time f msg =
        let stopwatch = Diagnostics.Stopwatch()
        stopwatch.Start()
        f ()
        stopwatch.Stop()
        printfn "Time taken for %s was %ims" msg stopwatch.ElapsedMilliseconds

    let wordCountViaAddText () =
        document ()
        |> List.reduce addText
        |> wordCount
        |> printfn "The word count is %i"

    time wordCountViaAddText "reduce then count"

    let wordCountViaMap () =
        document ()
        |> List.map wordCount
        |> List.reduce (+)
        |> printfn "The word count is %i"

    time wordCountViaMap "map then reduce"

    let wordCountViaParallelAddCounts () =
        document ()
        |> List.toArray
        |> Array.Parallel.map wordCount
        |> Array.reduce (+)
        |> printfn "The word count is %i"

    time wordCountViaParallelAddCounts "parallel map then reduce"

//most frequent words.

open System
open System.Text.RegularExpressions

type Text = Text of string

let addText (Text s1) (Text s2) = Text(s1 + s2)

let mostFrequentWord (Text s) =
    Regex.Matches(s, @"\S+")
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.ToString())
    |> Seq.groupBy id
    |> Seq.map (fun (k, v) -> k, Seq.length v)
    |> Seq.sortBy (fun (_, v) -> -v)
    |> Seq.head
    |> fst

let page1 () =
    List.replicate 1000 "hello world "
    |> List.reduce (+)
    |> Text

let page2 () =
    List.replicate 1000 "goodbye world "
    |> List.reduce (+)
    |> Text

let page3 () =
    List.replicate 1000 "foobar "
    |> List.reduce (+)
    |> Text

let document () = [ page1 (); page2 (); page3 () ]

document ()
|> List.reduce addText
|> mostFrequentWord
|> printfn "Using add first, the most frequent word is %s "

document ()
|> List.map mostFrequentWord
|> List.reduce (+)
|> printfn "Using map reduce, the most frequent word is %s " //This doesn't work as
// this isn't a monoid homomorphism

let wordFreq (Text s) =
    Regex.Matches(s, @"\S+")
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.ToString())
    |> Seq.groupBy id
    |> Seq.map (fun (k, v) -> k, Seq.length v)
    |> Map.ofSeq

// show some word frequency maps
page1 ()
|> wordFreq
|> printfn "The frequency map for page1 is %A"

page2 ()
|> wordFreq
|> printfn "The frequency map for page2 is %A"

//The frequency map for page1 is map [("hello", 1000); ("world", 1000)]
//The frequency map for page2 is map [("goodbye", 1000); ("world", 1000)]

document ()
|> List.reduce addText
|> wordFreq
|> printfn "The frequency map for the document is %A"

let addMap map1 map2 =
    let increment mapSoFar word count =
        match mapSoFar |> Map.tryFind word with
        | Some count' -> mapSoFar |> Map.add word (count + count')
        | None -> mapSoFar |> Map.add word count

    map2 |> Map.fold increment map1

let mostFrequentWord2 map =
    let max (candidateWord, maxCountSoFar) word count =
        if count > maxCountSoFar then (word, count) else (candidateWord, maxCountSoFar)

    map |> Map.fold max ("None", 0)

document ()
|> List.reduce addText
|> wordFreq
// get the most frequent word from the big map
|> mostFrequentWord2
|> printfn "Using add first, the most frequent word and count is %A"

//Using add first, the most frequent word and count is ("world", 2000)

document ()
|> List.map wordFreq
|> List.reduce addMap


// get the most frequent word from the merged smaller maps
|> mostFrequentWord2
|> printfn "Using map reduce, the most frequent and count is %A"

[ "The cat sat on the mat" |> Text
  "The quick brown fox jumped over the something or other"
  |> Text ]
|> List.map wordFreq
|> List.reduce addMap

let s =
    "The quick brown fox jumped over the something or other"

Regex.Matches(s, @"\S+")
|> Seq.cast<Match>
|> Seq.map (fun m -> m.ToString())
|> Seq.groupBy id
|> Seq.map (fun (k, v) -> k, Seq.length v)
|> Map.ofSeq
