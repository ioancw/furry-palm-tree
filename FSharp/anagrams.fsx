open System.Collections.Generic
open System.IO

let divideIntoEquivalenceClasses keyf seq =
    let dict = Dictionary<'key, ResizeArray<'T>>(HashIdentity.Structural)

    seq
    |> Seq.iter (fun v ->
        let key = keyf v
        let ok, value = dict.TryGetValue(key)

        if ok then
            value.Add(v) //add item to existing list
        else
            let prev = new ResizeArray<'T>()
            dict.[key] <- prev
            prev.Add(v))

    dict

let rec convertListToString l =
    match l with
    | [] -> ""
    | head :: tail -> head.ToString() + convertListToString tail

let sortString s =
    s |> Seq.sort |> Seq.toList |> convertListToString

let fileName = @"/Users/ioanwilliams/github/furry-palm-tree/words.txt"

let anagramDict =
    divideIntoEquivalenceClasses sortString (File.ReadAllLines fileName)

let printTopN (anagrams: Dictionary<string, ResizeArray<'T>>) n =
    anagrams
    |> Seq.sortByDescending (fun group -> group.Value.Count)
    |> Seq.take n
    |> Seq.iter (fun group ->
        group.Value
        |> Seq.map (fun s -> s.ToString())
        |> String.concat ","
        |> printfn "%s : %s" group.Key)

printTopN anagramDict 10

open System.IO

let data =
    (File.ReadAllLines fileName)
    |> Array.groupBy sortString
    |> Seq.sortByDescending (snd >> Array.length)
    |> Seq.take 10
    |> Seq.iter (fun entry ->
        snd entry
        |> Seq.map (fun s -> s.ToString())
        |> String.concat ","
        |> printfn "%s : %s" (fst entry))
