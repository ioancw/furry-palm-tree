//taken from https://isthisit.nz/posts/2020/cellular-automata-in-fsharp/
type Cell =
    | Empty
    | Full

type Rule = (Cell * Cell * Cell) -> Cell

let rule222 =
    function
    | Full, Full, Full -> Full
    | Full, Full, Empty -> Full
    | Full, Empty, Full -> Empty
    | Full, Empty, Empty -> Full
    | Empty, Full, Full -> Full
    | Empty, Full, Empty -> Full
    | Empty, Empty, Full -> Full
    | Empty, Empty, Empty -> Empty

let generateStandardFirstRow (width: int) =
    if width % 2 = 0 then
        invalidArg "width" (sprintf "Value must be odd: %d" width)

    Seq.init
        width
        (function
        | n when n = (width / 2) -> Full
        | _ -> Empty)

generateStandardFirstRow 7

let generateNextRow (rule: Rule) (row: seq<Cell>) =
    let generatedCells =
        row
        |> Seq.windowed 3
        |> Seq.map (fun v -> rule (v.[0], v.[1], v.[2]))

    seq {
        yield Empty
        yield! generatedCells
        yield Empty
    }

let generatePattern (rule: Rule) (firstRow: seq<Cell>) =
    firstRow
    |> Seq.unfold (fun row -> Some(row, (generateNextRow rule row)))

let firstRow = generateStandardFirstRow 101

let rows = generatePattern rule222 firstRow |> Seq.take 10

let drawCell = function 
    | Full -> "X"
    | Empty -> "."

let drawGrid (grid: seq<seq<Cell>>) =
    grid 
    |> Seq.map (fun row ->
        row
        |> Seq.map drawCell
        |> String.concat "")
    |> String.concat "\n"

drawGrid rows

let rule90: Rule = function
    | (Full, Full, Full) -> Empty
    | (Full, Full, Empty) -> Full
    | (Full, Empty, Full) -> Empty
    | (Full, Empty, Empty) -> Full
    | (Empty, Full, Full) -> Full
    | (Empty, Full, Empty) -> Empty
    | (Empty, Empty, Full) -> Full
    | (Empty, Empty, Empty) -> Empty
    
let firstRow1 = generateStandardFirstRow 101
let rows1 = generatePattern rule90 firstRow |> Seq.take 50
let arrayOfRows = rows |> Seq.toArray
    
drawGrid rows1

#r "nuget: SixLabors.ImageSharp"
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats

let cols' = 501
let rows' = 250

let image = new Image<Rgba32>(cols', rows')
let white = Rgba32(255F, 255F, 100F, 1F)
let black = Rgba32(0f, 0f, 0f, 1F)

let grid = 
    generateStandardFirstRow cols' 
    |> generatePattern rule90 
    |> Seq.take rows'

let withIndexes x = x |> Seq.mapi (fun index item -> (index, item))

for (rowIndex, row) in withIndexes grid do 
    for (cellIndex, cell) in withIndexes row do 
        let colour = 
            match cell with 
            | Full -> black 
            | Empty -> white    
        image.[cellIndex, rowIndex] <- colour

image.Save("/Users/ioanwilliams/github/furry-palm-tree/FSharp/rule90.png")