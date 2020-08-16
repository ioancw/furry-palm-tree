<<<<<<< HEAD
﻿module Query
=======
﻿module query
>>>>>>> 02237e5325edb20892e021cf36cccd16644d719b

open System
open System.IO

open Common
open Index
open Search

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

    let qt = queryTokens

    matchingFiles
    |> Seq.map (fun (Path path) -> path)
<<<<<<< HEAD
    |> search (queryTokens: Token []) id (fun (_, l) ->
           qt //TODO: need to sort this out - it should come from the tokens passed into this function
           |> Array.exists (fun (Token t) -> l.Contains(t)))
    |> Seq.groupBy (fun r -> r.File)
    |> printResults

let q =
    { QueryText = "agent"
      Index = getOrCreateIndex @"C:\Users\ioan_\GitHub" }

let ts, fs =
    match q |> runQuery with
    | QTokens (ql, l) -> ql, l
    | _ -> failwith "Query didn't execute correctly."
=======
    |> search id (fun (_, l: string) ->
           queryTokens
           |> Array.exists (fun (Token t) -> l.Contains(t)))
    |> Seq.groupBy (fun r -> r.File)
    |> printResults
>>>>>>> 02237e5325edb20892e021cf36cccd16644d719b
