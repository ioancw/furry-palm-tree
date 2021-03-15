//day 1 part 1

open System.IO

let d1p1 =
    File.ReadAllText(@"C:\Users\ioan_\GitHub\furry-palm-tree\FSharp\aoc-2015\d1_p1.txt")

let t1 = "(())"

let getFloor instruction =
    instruction
    |> Seq.fold (fun state floor ->
        match floor with
        | '(' -> state + 1
        | ')' -> state - 1
        | _ -> failwith "Unexpected Input") 0

let check expected actual = expected = actual

getFloor "(())" |> check 0
getFloor "(((" |> check 3
getFloor "(()(()(" |> check 3
getFloor "))(((((" |> check 3
getFloor "())" |> check -1
getFloor "))(" |> check -1
getFloor ")))" |> check -3
getFloor ")())())" |> check -3

//answer
getFloor d1p1

//day 1 part 2

let getFirstBasementPosition instruction =
    let rec processInst i acc insts =
        match acc with
        | -1 -> i, acc
        | _ ->
            match insts with
            | [] -> i, acc
            | '(' :: t -> processInst (i + 1) (acc + 1) t
            | ')' :: t -> processInst (i + 1) (acc - 1) t
            | _ -> failwith "Unexpected Input"

    processInst 0 0 (instruction |> Seq.toList)

getFirstBasementPosition ")" |> check (1, -1)
getFirstBasementPosition "()())" |> check (5, -1)

//answer
getFirstBasementPosition d1p1

//day 2 part 1

let getDimensions (dimensions: string) =
    let dims = dimensions.Split([| 'x' |])
    dims.[0] |> int, dims.[1] |> int, dims.[2] |> int

let calculateArea (l, w, h) =
    let area = [ l * w; w * h; h * l ]
    (area |> List.sum) * 2 + (area |> List.min)

let calculate (inputDimensions: string) =
    let area = getDimensions >> calculateArea
    inputDimensions |> area

calculate "1x1x10" |> check 43
calculate "2x3x4" |> check 58

let d2p1 =
    File.ReadAllLines(@"C:\Users\ioan_\GitHub\furry-palm-tree\FSharp\aoc-2015\d2_p1.txt")

let d2p1Answer = d2p1 |> Array.sumBy calculate

let calculateRibbon (l, w, h) =
    let minimumCircumferance =
        [ (l + w); (w + h); (l + h) ] |> List.min

    let volume = l * w * h
    2 * minimumCircumferance + volume

let calculateRibbonLength (inputDimensions: string) =
    let length = getDimensions >> calculateRibbon
    inputDimensions |> length

calculateRibbonLength "2x3x4" |> check 34
calculateRibbonLength "1x1x10" |> check 14

let d2p2Answer =
    d2p1 |> Array.sumBy calculateRibbonLength

//day 3 part 1

let east (x, y) = (x + 1, y)
let west (x, y) = (x - 1, y)
let north (x, y) = (x, y + 1)
let south (x, y) = (x, y - 1)

let move direction visited =
    let newPosition = visited |> List.head |> direction
    newPosition :: visited

let rec processMoves moves visited =
    match moves with
    | [] -> visited
    | '>' :: remainingMoves ->
        visited
        |> move east
        |> processMoves remainingMoves
    | '<' :: remainingMoves ->
        visited
        |> move west
        |> processMoves remainingMoves
    | '^' :: remainingMoves ->
        visited
        |> move north
        |> processMoves remainingMoves
    | 'v' :: remainingMoves ->
        visited
        |> move south
        |> processMoves remainingMoves
    | _ -> failwith "not instruction"

let processInstructions instructions =
    processMoves (instructions |> Seq.toList) [ (0, 0) ]

let countCommon visited =
    visited
    |> List.groupBy id
    |> List.map (fun (k, v) -> k, v |> List.length)
    |> List.filter (fun (k, c) -> c >= 1)
    |> List.length

let calculateHouses instructions =
    instructions |> processInstructions |> countCommon

calculateHouses ">" |> check 2
calculateHouses "^>v<" |> check 4
calculateHouses "^v^v^v^v^v" |> check 2

let d3p1 =
    File.ReadAllText(@"C:\Users\ioan_\GitHub\furry-palm-tree\FSharp\aoc-2015\d3_p1.txt")

let d3p1Answer = d3p1 |> calculateHouses

type Filter =
    | Odd
    | Even

let every f seq =
    seq
    |> Seq.mapi (fun i el -> el, i + 1) // Add index to element
    |> Seq.filter (fun (el, i) ->
        match f with
        | Filter.Odd -> i % 2 <> 0
        | Filter.Even -> i % 2 = 0) // Take every nth element
    |> Seq.map fst

let calculateHousesWithRobot instructions =
    let santa = instructions |> every Odd
    let robot = instructions |> every Even

    List.append (santa |> processInstructions) (robot |> processInstructions)
    |> countCommon

calculateHousesWithRobot "^v^v^v^v^v" |> check 11
calculateHousesWithRobot "^v" |> check 3
calculateHousesWithRobot "^>v<" |> check 3

let d3p3Answer = d3p1 |> calculateHousesWithRobot

// day 4 part 1

let substring position (input: string) = input.Substring(0, position)
let key = "abcdef"
let number = 609043
let plainText = key + number.ToString()

open System.Text
open System.Security.Cryptography

let md5 (input: string): string =
    let data =
        System.Text.Encoding.ASCII.GetBytes(input)

    use md5 = MD5.Create()
    (StringBuilder(), md5.ComputeHash(data))
    ||> Array.fold (fun sb b -> sb.Append(b.ToString("x2")))
    |> string

let x = plainText |> md5

let ifMatched = x.Substring(0, 5) = "00000"

let firstNCharacters N = substring N
let equalsFiveZeros input = input = "00000"

let results nZeroes inputKey =
    Seq.initInfinite id
    |> Seq.filter (fun i ->
        inputKey + i.ToString()
        |> md5
        |> firstNCharacters nZeroes
        |> equalsFiveZeros)
    |> Seq.head


let matcher n = 
    n, fun input -> input = "000000"


results 5 "iwrupvqb"
results 6 "iwrupvqb"
