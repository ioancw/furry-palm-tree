open System
open System.IO

let path =
    "/Users/ioanwilliams/Downloads/sequence.fasta"

let file = File.ReadAllText(path)
let totalLength = file.Length
let prefixLength = 97
let suffixLength = 2

let stringSequence =
    file.Substring(prefixLength, totalLength - prefixLength - suffixLength)

type Base =
    | A
    | C
    | G
    | T

let toBase (c: Char) = 
    match c with 
    | 'A' -> Some A 
    | 'C' -> Some C 
    | 'T' -> Some T 
    | 'G' -> Some G 
    | _ -> None 

let bases = 
    stringSequence.ToCharArray()
    |> Seq.map toBase
    |> Seq.filter (fun b -> b.IsSome)
    |> Seq.map (fun b -> b.Value)