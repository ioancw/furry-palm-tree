open System
open System.IO
open System.Text.RegularExpressions

let (|File|Directory|) path =
    if (Directory.Exists path) then Directory(path) else File(path)

let getFileSystemEntries path =
    Directory.GetFileSystemEntries path
    |> Array.toList

let getFiles path =
    let rec inner fileSystemEntries files =
        match fileSystemEntries with
        | [] -> files
        | File path :: rest -> inner rest (path :: files)
        | Directory path :: rest -> inner (rest @ (getFileSystemEntries path)) files

    inner (getFileSystemEntries path) []

let words s =
    Regex.Matches(s, @"\S+")
    |> Seq.cast<Match>
    |> Seq.map (fun x -> x.Value.ToLower())

let wordCount2 path =
    getFiles path
    |> Seq.map (File.ReadAllText >> words)
    |> Seq.reduce Seq.append
    |> Seq.groupBy id
    |> Seq.map (fun (l, ls) -> l, Seq.length ls)
    |> Seq.sortByDescending snd

#time "on"

let count2 =
    wordCount2 @"C:\Users\ioan_\GitHub\furry-palm-tree\FSharp\Books\books"

count2
