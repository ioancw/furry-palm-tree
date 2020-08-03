let cipherText =
    "MHILY LZA ZBHL XBPZXBL MVYABUHL HWWPBZ JSHBKPBZ JHLJBZ KPJABT HYJHUBT LZA ULBAYVU"

let alphabet = [ 'a' .. 'z' ]
let shiftAlphabet = [ 'A' .. 'Z' ]

let rotateListBy (offset: int) (input: char list) = input.[offset..] @ input.[..offset - 1]

let rec convertListToString list =
    match list with
    | [] -> ""
    | head :: tail -> head.ToString() + convertListToString tail

let allShifts =
    [ for i in [ 1 .. 26 ] do
        yield rotateListBy i alphabet
              |> List.zip shiftAlphabet
              |> Map.ofList ]

allShifts
|> List.iter (fun map ->
    cipherText
    |> Seq.map (fun x ->
        match (Map.tryFind x map) with
        | Some y -> y
        | None -> ' ')
    |> Seq.toList
    |> convertListToString
    |> printfn "%s")
