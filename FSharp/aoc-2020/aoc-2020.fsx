#r "nuget: Deedle"

open System
open System.IO

Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

[<AutoOpen>]
module Common =
    type Files() =
        member _.Item
            with get file = $"d{file}.txt"

    let Files = Files()

// day 1
let test = [ 1721; 979; 366; 299; 675; 1456 ]

let input =
    System.IO.File.ReadLines Files.[1]
    |> Seq.map int
    |> Seq.toList

let data input =
    seq {
        for i in input do
            for j in input do
                i + j, i * j
    }

let answer data check =
    input
    |> data
    |> Seq.filter (fun item -> fst item = check)
    |> Seq.head
    |> snd

// part 1
answer data 2020

// part 2
let data2 input =
    seq {
        for i in input do
            for j in input do
                for k in input do
                    i + j + k, i * j * k
    }

answer data2 2020

// day 2
let test2 =
    [ "1-3 a: abcde"
      "1-3 b: cdefg"
      "2-9 c: ccccccccc" ]

type Policy =
    { Lower: int
      Upper: int
      Letter: char
      Password: string }

let parser (policy: string) =
    let items = policy.Split([| ' ' |])
    let password = items.[2]
    let letter = items.[1].[0]
    let minMax = items.[0].Split([| '-' |])
    let lower = minMax.[0]
    let upper = minMax.[1]
    { Lower = lower |> int
      Upper = upper |> int
      Letter = letter |> char
      Password = password }

let validatePolicy policy =
    let letterCount =
        policy.Password
        |> Seq.filter ((=) policy.Letter)
        |> Seq.length

    letterCount
    >= policy.Lower
    && letterCount <= policy.Upper

test2
|> List.map parser
|> List.map validatePolicy

let input2 = File.ReadLines Files.[2]

input2
|> Seq.map parser
|> Seq.map validatePolicy
|> Seq.filter ((=) true)
|> Seq.length

let validatePolicy2 policy =
    let letter = policy.Letter
    let password = policy.Password
    let first = policy.Lower
    let second = policy.Upper
    match password.[first - 1] = letter, password.[second - 1] = letter with
    | true, false
    | false, true -> true
    | _ -> false

input2
|> Seq.map parser
|> Seq.map validatePolicy2
|> Seq.filter ((=) true)
|> Seq.length

// day 3
let testMap =
    [ "..##.........##.........##.........##.........##.........##......."
      "#..O#...#..#...#...#..#...#...#..#...#...#..#...#...#..#...#...#.."
      ".#....X..#..#....#..#..#....#..#..#....#..#..#....#..#..#....#..#."
      "..#.#...#O#..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#"
      ".#...##..#..X...##..#..#...##..#..#...##..#..#...##..#..#...##..#."
      "..#.##.......#.X#.......#.##.......#.##.......#.##.......#.##....."
      ".#.#.#....#.#.#.#.O..#.#.#.#....#.#.#.#....#.#.#.#....#.#.#.#....#"
      ".#........#.#........X.#........#.#........#.#........#.#........#"
      "#.##...#...#.##...#...#.X#...#...#.##...#...#.##...#...#.##...#..."
      "#...##....##...##....##...#X....##...##....##...##....##...##....#"
      ".#..#...#.#.#..#...#.#.#..#...X.#.#..#...#.#.#..#...#.#.#..#...#.#" ]

let step = (3, 1)

let path =
    (0, 0) |> Seq.unfold (fun s -> Some(s, (fst s + fst step, snd s + snd step)))  
    |> Seq.take 5 |> Seq.tail

let row = testMap.[1]
let cell = row.[8]
path 
|> Seq.map (fun (x, y) -> 
    let row = testMap.[y]
    let position = row.[x-1]
    if position = '#' then true else false)