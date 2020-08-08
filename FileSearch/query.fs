module query

open System
open System.IO
open common
open Index
open search

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
        QTokens(Array.append ql qr, evalResult)
        :: t
        |> processTokens
    | _ when (qs.Length % 2) = 0 -> failwith "Not a balanced list"
    | _ -> failwith (sprintf "Error: %A" qs)

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
    |> search id (fun (_, l: string) ->
           queryTokens
           |> Array.exists (fun (Token t) -> l.Contains(t)))
    |> Seq.groupBy (fun r -> r.File)
    |> printResults
