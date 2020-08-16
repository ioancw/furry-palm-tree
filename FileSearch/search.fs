module Search

open System
open System.IO
open System.Text.RegularExpressions

open Common

//found list of files to 'examine' further in order to find the line numbers where the search words exist.
type SearchResult(lineNo: int, file: string, content: string, tokens: Token []) =
    member t.LineNo = lineNo
    member t.File = file
    member t.Content = content
    member t.Tokens = tokens

let read (file: string) =
    seq {
        use reader = new StreamReader(file)
        while not reader.EndOfStream do
            reader.ReadLine()
    }

let searchFile tokens parse check file =
    read file
    |> Seq.mapi (fun i l -> i + 1, parse l)
    |> Seq.filter check
    |> Seq.map (fun (i, l) -> SearchResult(i, file, l, tokens))

let search tokens parse check =
    Seq.map (fun file -> searchFile tokens parse check file)
    >> Seq.concat

let printLineWithHighlight line tokens =
    //In order to do a Regex.Split (and return the word you split on) you need
    //to form the regex string as "(Dave)|(David)""
    let regex =
        tokens
        |> Array.map
            ((fun (Token t) -> t)
             >> (fun t -> sprintf "(%s)" t))
        |> stringJoinS "|"

    Regex.Split(line, regex)
    |> Array.iter (fun s ->
        if Array.contains (Token s) tokens then printc Colours.magenta s else printc Colours.darkgreen s)
    printc Colours.darkyellow (sprintf "\n")

let printResults: seq<string * seq<SearchResult>> -> unit =
    do apply Colours.yellow
    //group search results
    Seq.fold (fun c (path, results) ->
        printc Colours.darkyellow (sprintf "%A\n" path)
        let size = results |> Seq.length
        results
        |> Seq.iter (fun sr ->
            printc Colours.gray (sprintf "line %d" sr.LineNo)
            printLineWithHighlight sr.Content sr.Tokens)
        c + size) 0
    >> printfn "%d results"
