open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic

let fileName = "day6.txt"

let data =
    Path.Combine(__SOURCE_DIRECTORY__, fileName)
    |> File.ReadAllLines
    |> List.ofArray
    
let testData =
    [
        "mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7
        "bvwbjplbgvbhsrlpgdmjqwftvncz", 5
        "nppdvjthqldpwncqszvftbrmjlhg", 6
        "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10
        "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11
    ]
    
let parse letterCount input =
    input
    |> Seq.mapi (fun i s -> i, s)
    |> Seq.windowed letterCount
    |> Seq.map (fun ws -> ws |> Seq.map fst, ws |> Seq.map snd |> Set.ofSeq)
    |> Seq.filter (fun (_, s) -> Set.count s = letterCount)
    |> Seq.head
    |> (fun (is, _) -> is |> Seq.last |> (+) 1)
    
// part 1    
testData |> List.map (fun (t, c) -> parse 4 t = c)
data |> List.map (fun s -> s |> parse 4)

//part 2
let testData2 =
    [
        "mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19
        "bvwbjplbgvbhsrlpgdmjqwftvncz", 23
        "nppdvjthqldpwncqszvftbrmjlhg", 23
        "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29
        "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26
    ]
    
testData2 |> List.map (fun (t, c) -> parse 14 t = c)
data |> List.map (fun s -> s |> parse 14)