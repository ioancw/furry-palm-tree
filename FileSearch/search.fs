
module search
open common
open System
open System.IO

//found list of files to 'examine' further in order to find the line numbers where the search words exist.
type SearchResult<'a>(lineNo: int, file: string, content: 'a) =
    member t.LineNo = lineNo
    member t.File = file
    member t.Content = content
    override this.ToString() = sprintf "line %d in %s – %O" this.LineNo this.File this.Content

let read (file: string) =
    seq {
        use reader = new StreamReader(file)
        while not reader.EndOfStream do
            reader.ReadLine()
    }

//module results
let searchFile parse check file =
    read file
    |> Seq.mapi (fun i l -> i + 1, parse l)
    |> Seq.filter check
    |> Seq.map (fun (i, l) -> SearchResult(i, file, l))

let search parse check =
    Seq.map (fun file -> searchFile parse check file)
    >> Seq.concat

let printResults<'a> : seq<'a * seq<'a SearchResult>> -> unit =
    do apply Colours.yellow
    //group search results
    Seq.fold (fun c (path, results) ->
        printc Colours.darkyellow (sprintf "%A\n" path)
        let size = results |> Seq.length
        results
        |> Seq.iter (fun sr -> 
            printc Colours.gray (sprintf "line %d" sr.LineNo)
            printc Colours.darkgreen (sprintf "%O\n" sr.Content))
        c + size) 0
    >> printfn "%d results"



let lineSplitter (s: string) = s.Split(' ')
let qTokens = ["on"; "thr"]

let l = "one two three" 
let p = 
    l
    |> lineSplitter
    |> Array.map(fun w -> 
                    let n = 
                        qTokens
                        |> List.map ( fun t ->
                            if w.Contains(t) then
                                sprintf "Red{%s}" w
                            else
                                w)
                    n)
String.Join(" ", p)

let tokens = ["on"; "thr"]
let words = "one two three" |> lineSplitter
let result = 
    seq {
        for w in words do
            if List.exists (fun (x: string) -> w.Contains(x)) tokens then
                sprintf "Red{%s}" w
            else w
    }

let result2 = 
    let colourtagged = 
        words
        |> Array.map (fun w ->
            if List.exists (fun (x: string) -> w.Contains(x)) tokens then
                sprintf "Red{%s}" w
            else w
            )
    String.Join(" ", colourtagged)
