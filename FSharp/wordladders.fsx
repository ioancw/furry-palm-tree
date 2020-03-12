open System.Collections.Generic
open System.IO

//word ladders
let fileName2 = @"/Users/ioanwilliams/github/furry-palm-tree/words.txt"

let rotateBy (offset: int) (input: list<string>) = input.[..offset-1] @ ["_"] @ input.[offset+1..] 

let stringToList (s: string) = 
    s 
    |> Seq.map (string) 
    |> Seq.toList

let rec convertToString (l) = 
    match l with
    | [] -> ""
    | head :: tail -> head.ToString() + convertToString tail

let getWildcards (word: string) = 
    word
    |> Seq.mapi(fun i _ -> rotateBy i (stringToList word) |> convertToString)
    |> List.ofSeq

let getWordDics seq = 
    let dict = Dictionary<string, ResizeArray<string>>(HashIdentity.Structural)
    seq
    |> Seq.iter (fun v ->
        getWildcards v
        |> List.iter( fun key -> 
            let ok, value = dict.TryGetValue(key)
            if ok 
                then value.Add(v) //add item to existing list
            else 
                let prev = new ResizeArray<string>()
                dict.[key] <- prev  
                prev.Add(v))
                )      
    dict  

let data = 
    (File.ReadAllLines fileName2)
    |> Array.groupBy String.length
    
let getGraph words = 
    let wordDict = getWordDics words
    let dict = Dictionary<string, string list>(HashIdentity.Structural)
    let addToDict k words = 
        let ok, value = dict.TryGetValue(k)
        if not ok then
            dict.[k] <- words
    
    words
    |> Array.iter (fun word ->
                    let matching = 
                        getWildcards word
                        |> List.fold (fun n input -> 
                                          let ns = wordDict.[input] 
                                                   |> List.ofSeq
                                                   |> List.filter (fun n -> n <> word)
                                          n @ ns)
                                     ([]) 
                    addToDict word matching)
    dict

let breadthFirstSearch2 (edgesFrom: Dictionary<string, string list>) v t =
    seq{let queue = Queue<_>()
        queue.Enqueue [v]
        let visited = Set.empty
        while queue.Count > 0 do
          let path = queue.Dequeue()
          let v = path.Head
          if v <> t then
              visited.Add v |> ignore
              Set.difference (edgesFrom.[v] |> Set.ofList) visited
              |> Set.iter (fun item -> queue.Enqueue ([item] @ path))
          else
              yield (v, path)}

let printWords (wrd: string) (wordDict: Dictionary<string, ResizeArray<string>>) = 
    getWildcards wrd
    |> List.iter (fun (w: string) -> 
                    let words = wordDict.[w] |> List.ofSeq |> List.filter (fun n -> n <> wrd)
                    do printfn "%s:%A" w words)

let getWordLadder (startWord: string) endWord = 
    //assume for now length of both words are the same
    let mainGraph = snd data.[startWord.Length] |> getGraph

    breadthFirstSearch2 mainGraph startWord endWord 
    |> Seq.take 10 |> List.ofSeq

getWordLadder "fail" "work"
                                                      