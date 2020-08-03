(*@
layout: post
title: "Test F#"
tags: [binpacking]
description: "Bin Packing"
keywords: F#, bin, packing
*)

(**
Bin packing with F#
==============================================
This is my post to simulate packing certain items into a bin.
I read the following in a paper trying to optimise (i.e lower) the numbers of bins (or boxes)
used to pack items.

What I've done, is simulate packing 90 randomly chosen boxes, that range in volume between 1 and 1,000,000.
I've run the simulation 1,000,000 times, and provided the average number of boxes used to pack the 90 items.
---------------------------------------------------------------------------------------------------
*)

(*** hide ***)
#r "/Users/ioanwilliams/FSharp/packages/FSharp.Collections.ParallelSeq/lib/net45/FSharp.Collections.ParallelSeq.dll"

open FSharp.Collections.ParallelSeq
open System

let packBoxes volume items= 
    let mutable boxes = [||]
    items
    |> Array.sortDescending
    |> Array.iter (fun box ->
                    let sackIndex = boxes |> Array.tryFindIndex (fun sack -> sack + box <= volume)
                    
                    match sackIndex with
                    | Some index -> boxes.[index] <- boxes.[index] + box
                    | None -> 
                        Array.Resize(&boxes, Array.length boxes + 1)
                        boxes.[Array.length boxes - 1 ] <- box
                )
    Array.length boxes

let randomItems from upTo times =
    let randy = Random()
    Array.init times (fun _ -> randy.Next(from, upTo))

let stopWatch = System.Diagnostics.Stopwatch.StartNew()
let vol = 1000000
let packingSim = packBoxes vol
let randomiser= randomItems 1 vol 90
let simulation4 = PSeq.fold (fun acc _ -> (randomiser |> packingSim) + acc) 0 {0 .. vol}


//finds average number of boxes used when 1m simulations are run

stopWatch.Stop()

let averageSacks = (float)simulation4 / (float)vol
printfn "Average number of sacks used %f" averageSacks
printfn "Number of seconds%i" stopWatch.Elapsed.Seconds

