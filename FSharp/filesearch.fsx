open System
open System.IO

//Index Module
type Token = Token of string
type Path = Path of string
type Ngram = Ngram of string
type Document = { Path: Path; Tokens: Token [] }

type Index =
    { Ngrams: Map<Ngram, Token []>
      Tokens: Map<Token, Path []> }

type Query = { QueryText: string; Index: Index }

let fileTypeToIndex = "fs"

let getFiles fileTypeToIndex folderToIndex =
    Directory.GetFiles(folderToIndex, "*." + fileTypeToIndex, SearchOption.AllDirectories)

let delimiters =
    ".,;<>()-+!@#$%^&*?[]{};= \t\0'\"\\/".ToCharArray()

let stopWords =
    [ "namespace"; "open"; "let" ] |> List.map Token

let stringParse delimiter (s: string) = s.Split([| delimiter |])
let parseTilde = stringParse '~'
let parseColon = stringParse ':'
let parseComma = stringParse ','

let getDocuments (fileNames: string array) =
    fileNames
    |> Array.map (fun fileName ->
        let tokens =
            File.ReadAllLines(fileName)
            |> Array.collect (fun line ->
                line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                |> Array.map Token)
            |> Array.filter (fun token -> not <| List.contains token stopWords)

        { Path = Path(fileName)
          Tokens = tokens |> Array.distinct })

let tokeniseDocuments = getFiles fileTypeToIndex >> getDocuments

let distinctSnd (f, s) =
    (f, s |> Set.ofArray |> Seq.map snd |> Seq.toArray)

let documentPath =
    fun (k, ds) -> (k, ds |> Array.map (fun d -> d.Path))

let tokenToDocument (documents: Document []) =
    documents
    |> Array.collect (fun document ->
        document.Tokens
        |> Array.map (fun token -> (token, document)))
    |> Array.groupBy fst
    |> Array.map (distinctSnd >> documentPath)
    |> Map.ofArray

let ngrams n (Token t) =
    t
    |> Seq.windowed (n + 1)
    |> Seq.map (fun chars ->
        chars
        |> Array.take n
        |> Array.map string
        |> Array.reduce (+)
        |> Ngram)
    |> Seq.toArray

let ngramsToTokens (documents: Document []) =
    let n = 3
    let getNgrams = ngrams n
    documents
    |> Array.collect (fun document -> document.Tokens)
    |> Array.filter (fun (Token token) -> token.Length >= n)
    |> Array.collect (fun token ->
        token
        |> getNgrams
        |> Array.map (fun ngram -> (ngram, token)))
    |> Array.groupBy fst
    |> Array.map distinctSnd
    |> Map.ofArray

//QueryModule
let searchIndexForTerm index (searchTerm: string) =
    let searchNgram = searchTerm.Substring(0, 3) |> Ngram

    let matchedTokens =
        index.Ngrams.[searchNgram] //find all 'tokens' that match seach ngram
        |> Array.filter (fun (Token token) -> token.Contains(searchTerm))
    //wild card is Contains, i.e. search for let you get completely and deedletest
    //it's the token that should be returned
    //as if not you end up finding all the let words in those two files.
    let matchedDocs =
        matchedTokens
        |> Array.collect (fun token -> index.Tokens.[token]) //get the matching docs
        |> Set.ofArray

    matchedTokens, matchedDocs

//save
let saveTokens indexFolder tokens =
    let lines =
        tokens
        |> Map.map (fun (Token token) paths ->
            paths
            |> Array.map (fun (Path path) -> sprintf "%s~%s" token path))
        |> Map.toSeq
        |> Seq.collect snd

    do File.WriteAllLines(indexFolder + "/tokens.txt", lines)

let stringJoin (delimiter: char) (items: string []) = String.Join(delimiter, items)
let arrayJoinComma = stringJoin ','

let saveNgrams indexFolder (ngrams: Map<Ngram, Token []>) =
    let lines =
        ngrams
        |> Map.map (fun (Ngram ngram) tokens ->
            tokens
            |> Array.map (fun (Token t) -> t)
            |> arrayJoinComma
            |> sprintf "%s:%s" ngram)

        |> Map.toSeq
        |> Seq.map snd

    do File.WriteAllLines(indexFolder + "/ngrams.txt", lines)

let arraySecond (a: 'a []) = a.[1]

//load
let loadTokens indexFolder =
    let fileName = indexFolder + "/tokens.txt"
    if File.Exists(fileName) then
        File.ReadAllLines(fileName)
        |> Seq.groupBy (parseTilde >> Array.head >> Token)
        |> Seq.map (fun (token, lines) ->
            token,
            lines
            |> Seq.map (parseTilde >> arraySecond >> Path)
            |> Seq.toArray)
        |> Map.ofSeq
        |> Some
    else
        None

let loadNgrams indexFolder =
    let fileName = indexFolder + "/ngrams.txt"
    if File.Exists(fileName) then
        File.ReadAllLines(fileName)
        |> Seq.map (fun line ->
            let colonParsed = line |> parseColon
            colonParsed |> Array.head |> Ngram,
            colonParsed
            |> arraySecond
            |> parseComma
            |> Array.map Token)
        |> Map.ofSeq
        |> Some
    else
        None

let getOrCreateIndex indexFolder =
    //only want to execute when I need to
    let tokensisedDocuments = tokeniseDocuments indexFolder

    let ngrams =
        match loadNgrams indexFolder with
        | Some n -> n
        | None ->
            let ngrams = ngramsToTokens tokensisedDocuments
            saveNgrams indexFolder ngrams
            ngrams

    let tokens =
        match loadTokens indexFolder with
        | Some t -> t
        | None ->
            let tokens = tokenToDocument tokensisedDocuments
            saveTokens indexFolder tokens
            tokens

    { Ngrams = ngrams; Tokens = tokens }

type QOp =
    | And
    | Or

type QToken =
    | QTokens of Token [] * Set<Path>
    | QOperator of QOp

let queryTokens query =
    let executeQuery = searchIndexForTerm query.Index

    query.QueryText
    |> stringParse ' '
    |> Array.map (fun t ->
        match t with
        | "&" -> QOperator And
        | "|" -> QOperator Or
        | _ -> QTokens(executeQuery t))
    |> List.ofArray

let eval l o r =
    match o with
    | And -> Set.intersect l r
    | Or -> Set.union l r

let rec processTokens qs =
    match qs with
    | [ _ ] -> qs
    | QTokens (ql, l) :: QOperator o :: QTokens (qr, r) :: t ->
        let evalResult = eval l o r
        //let tokens = sprintf "%s,%s" ql qr
        QTokens(Array.append ql qr, evalResult)
        :: t
        |> processTokens
    | _ when (qs.Length % 2) = 0 -> failwith "Not a balanced list"
    | _ -> failwith (sprintf "Error: %A" qs)

//found list of files to 'examine' further in order to find the line numbers where the search words exist.
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
    //group search results
    Seq.fold (fun c (path, results) ->
        printfn "%A" path
        let size = results |> Seq.length
        results
        |> Seq.iter (fun sr -> printfn "line %d:%O" sr.LineNo sr.Content)
        c + size) 0
    >> printfn "%d results"

let runQuery =
    queryTokens >> processTokens >> List.head

let searchFor queryFolder query =
    let q =
        { QueryText = query
          Index = getOrCreateIndex queryFolder }

    let queryTokens, matchingFiles =
        match q |> runQuery with
        | QTokens (ql, l) -> ql, l
        | _ -> failwith "Query didn't execute correctly."

    matchingFiles
    |> Seq.map (fun (Path path) -> path)
    |> search id (fun (_, l) ->
           queryTokens
           |> Array.exists (fun (Token t) -> l.Contains(t)))
    |> Seq.groupBy (fun r -> r.File)
    |> printResults

let folderToIndex =
    @"/Users/ioanwilliams/github/furry-palm-tree/FSharp"

let searchFolder = searchFor folderToIndex
searchFolder "let"
