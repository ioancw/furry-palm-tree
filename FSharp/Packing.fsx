open System

type Sack() = 
    let boxes = ResizeArray<int>()
    
    member t.AddBox(item) = 
        boxes.Add(item)

    member t.Sum() = 
        boxes |> Seq.sum

let packSleigh boxes volume= 
    let sleigh = ResizeArray<Sack>()
    boxes
    |> Array.sortDescending
    |> Array.iter (fun box ->  
                    //first see if there is an empty sack, in which we can pack box
                    let addToSleigh = sleigh |> Seq.tryPick (fun sack -> 
                                                                if (sack.Sum() + box <= volume) 
                                                                then Some(sack) 
                                                                else None)
                    match addToSleigh with
                    | Some sack -> sack.AddBox(box) //add box to that sack
                    | None -> let sack = Sack() //otherwise open sack
                              sack.AddBox(box)
                              sleigh.Add(sack)
                )
    sleigh |> Seq.length

let packSleigh2 boxes volume= 
    let mutable sleigh = [|0|]
    boxes //90 boxes
    |> Array.sortDescending //sort them
    |> Array.iter (fun box ->  //work out if box is to be added to existing sack or new one
                    let sackIndex = sleigh |> Array.tryFindIndex (fun sack -> sack + box <= volume)
                    match sackIndex with
                    | Some index -> sleigh.[index] <- sleigh.[index] + box
                    | None -> sleigh <- Array.append sleigh [|box|]
                )
    Array.length sleigh

open System
/// Actual simulation
let randomBoxes from upTo times =
    let randy = Random()
    Array.init times (fun _ -> randy.Next(from, upTo))

let vol = 1000001
//let simulation = List.init vol (fun _ -> (packSleigh (randomBoxes 1 vol 90) vol)) //fast
//let simulationBoxes = List.init vol (fun _ -> randomBoxes 1 vol 90) 
//let averageSimulation = List.fold (fun acc boxes -> (packSleigh boxes vol) + acc) 0 simulationBoxes

//Real: 00:01:50.712, CPU: 00:01:50.798, GC gen0: 26176, gen1: 9
//low memory
//Real: 00:01:46.469, CPU: 00:01:46.604, GC gen0: 24616, gen1: 9
let billybox =   [|643088; 748498; 392790; 734652; 288471; 101166; 202038; 122564; 266151;
    744882; 750037; 752946; 264592; 607768; 90169; 7171; 813232; 796938;
    804558; 685130; 490741; 122348; 88264; 944886; 299423; 217107; 360530;
    223754; 258395; 127823; 64710; 898143; 612185; 664180; 152857; 794374;
    333107; 811087; 206381; 83123; 912575; 400063; 262566; 598301; 72905;
    339526; 138395; 75322; 23293; 152061; 300712; 706201; 970992; 178055;
    938658; 520741; 660235; 447904; 435230; 71365; 740637; 978285; 864170;
    138328; 680172; 851895; 140762; 600413; 454912; 295795; 674064; 2146;
    590557; 721435; 772556; 90679; 859782; 489963; 871982; 959897; 78712;
    285208; 200461; 106334; 827112; 358510; 927151; 434130; 725522; 632116|]

//let averageSimulation2 = Seq.fold (fun acc _ -> (packSleigh (randomBoxes 1 vol 90) vol) + acc) 0 {1 .. vol}
//Real: 00:00:22.988, CPU: 00:00:22.979, GC gen0: 1965, gen1: 0
//Real: 00:00:14.551, CPU: 00:00:14.547, GC gen0: 2702, gen1: 0
let averageSimulation3 = Seq.fold (fun acc _ -> (packSleigh2 billybox vol) + acc) 0 {1 .. vol} |> float

//let totalSum = simulation |> List.sum |> float
let averageSacks= averageSimulation3  / 1000001.0
printfn "Average number of sacks used %f" averageSacks

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

