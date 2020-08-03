open System
open System.IO

type SearchResult<'a>(lineNo: int, file: string, content: 'a) =
    member this.LineNo = lineNo
    member this.File = file
    member this.Content = content
    override this.ToString() = sprintf "line %d in %s – %O" this.LineNo this.File this.Content

let read (file: string) =
    seq {
        use reader = new StreamReader(file)
        while not reader.EndOfStream do
            reader.ReadLine()
    }

let searchFile parse check file =
    read file
    |> Seq.mapi (fun i l -> i + 1, parse l)
    |> Seq.filter check
    |> Seq.map (fun (i, l) -> SearchResult(i, file, l))

let search parse check =
    Seq.map (fun file -> searchFile parse check file)
    >> Seq.concat

let printResults<'a> : seq<'a * seq<'a SearchResult>> -> unit =
    //group search results
    Seq.fold (fun c (path, results) ->
        printfn "%A" path
        let size = results |> Seq.length
        results |> Seq.iter(fun sr -> printfn "line %d:%O" sr.LineNo sr.Content )
        c + size) 0
    >> printfn "%d results"

Directory.GetFiles(@"/Users/ioanwilliams/github/furry-palm-tree/FSharp", "*.fs", SearchOption.AllDirectories)
|> search id (fun (i, l) -> l.Contains("number"))
|> Seq.groupBy (fun r -> r.File)
|> printResults

"/Users/ioanwilliams/github/furry-palm-tree/FSharp/learnfsharp.fs"
|> searchFile id (fun (i, l) -> l.Contains("let"))
//maybe a two phased approach
//first phase uses ngrams and token to find the candidate files that match the required token
//we assume that the set of files returned will be small.
//we then search each returned file sequentially in order to find out which lines the token exists on
//that way we can get the line numbers
