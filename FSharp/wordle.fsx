open System.IO

let rec listToString l =
    match l with
    | [] -> ""
    | head :: tail -> head.ToString() + listToString tail

let sortString s =
    s |> Seq.sort |> Seq.toList |> listToString

let fileName = @"C:\Users\ioan_\GitHub\furry-palm-tree\words.txt"

let findMatchingFiveLetterWords word = 
    let fivemap = 
        fileName
        |> File.ReadAllLines
        |> Seq.groupBy sortString
        |> Seq.sortByDescending (snd >> Seq.length)
        |> Seq.filter (fun (k, vs) -> k.Length = 5)
        |> Map.ofSeq
    match Map.tryFind word fivemap with 
    | Some ws -> ws |> Seq.toArray
    | None -> Array.empty

let words = 
    fileName 
    |> File.ReadAllLines 
    |> Seq.filter (fun w -> w.Length = 5)
    |> Seq.collect string
    |> Seq.groupBy id
    |> Seq.map (fun (k, vs) -> string k, vs |> Seq.length)
    |> Seq.sortByDescending snd
    |> Seq.take 5
    |> Seq.map fst
    |> Seq.sort
    |> Seq.fold (+) ""
    |> findMatchingFiveLetterWords