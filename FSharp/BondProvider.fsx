#load "packages/FsLab/FsLab.fsx"

open FSharp.Data

let bondUrl = "https://en.wikipedia.org/w/index.php?title=List_of_James_Bond_films&oldid=688916363"
type BondProvider = HtmlProvider<"https://en.wikipedia.org/w/index.php?title=List_of_James_Bond_films&oldid=688916363">

let bondWiki = BondProvider.Load(bondUrl)

let boxOffice = 
    let allBoxOffice = 
        [| for row in bondWiki.Tables.``Box office``.Rows ->
            row.Title, 
            row.Year, 
            row.``Box office - Adjusted 2005 $ (millions)[19]``, 
            row.``Budget - Adjusted 2005 $ (millions)[19]``, 
            row.``Bond actor`` |]
    allBoxOffice.[1..allBoxOffice.Length - 3]
    |> Array.map (fun (titleRaw, yr, bdgt, bo, actorRaw) -> 
        let actor = actorRaw.[actorRaw.Length / 2 + 1 .. ]
        let title = 
            match titleRaw |> Seq.tryFindIndex ((=) '!') with
            | Some(idx) -> titleRaw.[idx + 1 ..]
            | None -> titleRaw
        titleRaw, int yr, float bdgt, float bo, actorRaw)

