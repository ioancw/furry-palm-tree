#r "/Users/ioanwilliams/FSharp/packages/FSharp.Collections.ParallelSeq/lib/net45/FSharp.Collections.ParallelSeq.dll"
open FSharp.Collections.ParallelSeq
open System

let packBoxes items volume= 
    let mutable boxes = [||] //this should be a tree, for easier searching.
    items
    |> Array.sortDescending
    |> Array.iter (fun box ->
                    let sackIndex = boxes |> Array.tryFindIndex (fun sack -> sack + box <= volume)
                    
                    match sackIndex with
                    | Some index -> boxes.[index] <- boxes.[index] + box
                    | None -> boxes <- Array.append boxes [|box|]
                )
    Array.length boxes

let randomItems from upTo times =
    let randy = Random()
    Array.init times (fun _ -> randy.Next(from, upTo))

let vol = 1000000
let simulation4 = PSeq.fold (fun acc _ -> (packBoxes (randomItems 1 vol 90) vol) + acc) 0 {0 .. vol}

let averageSacks = (float)simulation4  / (float)vol
printfn "Average number of sacks used %f" averageSacks


