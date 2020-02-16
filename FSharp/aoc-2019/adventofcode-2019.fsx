open System
open System.IO
let myAssert a b = a = b

// day 1.1
let calcMass n =  n / 3 - 2

assert (calcMass 12 = 4)

myAssert (calcMass 12) 2
myAssert (calcMass 14) 2
myAssert (calcMass 1969) 654
myAssert (calcMass 100756) 33583

let inputData fileName = 
    File.ReadAllLines fileName
    |> Seq.map int

let input = 
    @"/Users/ioanwilliams/github/furry-palm-tree/FSharp/aoc-2019/1_1.txt"
    |> inputData

let p1 = input |> Seq.sumBy calcMass

// day 1.2
let calcFuel n =
    let rec calc fuel ns = 
        match (calcMass fuel) with
        | x when x > 0 -> calc x (x::ns)
        | x -> ns
    calc n [] |> List.sum

calcFuel 100756

let p2 = input |> Seq.sumBy calcFuel