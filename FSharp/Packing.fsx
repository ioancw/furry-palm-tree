open System
//open System.Collections.Generic

type Sack() = 
    let boxes = ResizeArray<int>()
    
    member t.AddBox(item) = 
        boxes.Add(item)

    member t.Sum() = 
        boxes |> Seq.sum
    
    member t.Boxes() =
        boxes

let packSleigh boxes volume= 
    let sleigh = ResizeArray<Sack>()
    boxes
    |> List.sortDescending
    |> List.iter (fun box ->  
                    let addToSleigh = sleigh |> Seq.tryPick (fun sack -> 
                                                                if (sack.Sum() + box <= volume) 
                                                                then Some(sack) 
                                                                else None)
                    match addToSleigh with
                    | Some sack -> sack.AddBox(box) 
                    | None -> let sack = Sack()
                              sack.AddBox(box)
                              sleigh.Add(sack)
                )
    sleigh

/// Actual simulation
let randomBoxes from upTo times =
    let randy = Random()
    List.init times (fun _ -> randy.Next(from, upTo))

let vol = 1000001
let simulation = List.init vol (fun _ -> (packSleigh (randomBoxes 1 vol 90) vol))

let totalSum = simulation |> List.sumBy (fun sleigh  -> sleigh |> Seq.length) |> float
let averageSacks= totalSum / ( simulation |> Seq.length |> float)

/// Testing
//let boxesToPack = [1;2;3;4;5;6;7;8;9;10] 

//packSleigh boxesToPack 10
//|> Seq.iter (fun sack -> printfn "%i" (sack.Sum()))

//let packedSleigh = packSleigh boxesToPack 10
//let numberOfSacksUsed = packedSleigh |> Seq.length
//packedSleigh
//|> Seq.iter (fun sack -> 
//                sack.Boxes() |> Seq.iter (fun box -> printf "%i:" box)
//                printfn "~ Sum is %i" (sack.Sum()))

//what about the random function, we essentially want 90 random numbers between 1 and 1,000,000
//1million times.

//let simulationBoxes = List.init 1000001 (fun _ -> randomBoxes 1 1000001 90)

