let toRoman (one, five, ten) = function
    | 1 -> one
    | 2 -> one + one
    | 3 -> one + one + one
    | 4 -> one + five
    | 5 -> five
    | 6 -> five + one
    | 7 -> five + one + one
    | 8 -> five + one + one + one
    | 9 -> one + ten
    |  _  -> ""

let arabicToOrdinal arabic ordinal = arabic % (10 * ordinal) / ordinal

let arabicToOrdinals arabic = 
    let ordinal = arabicToOrdinal arabic
    [1000;100;10;1]
    |> List.map (fun o -> (o, ordinal o))

let romanDigits2 = Map.ofList [
    (1, ("I", "V", "X"))
    (10, ("X", "L","C"))
    (100, ("C", "D", "M"))
    (1000, ("M", "",""))] 

let convertArabicToRoman arabic = 
    arabicToOrdinals arabic
    |> List.map (fun (o, n) -> toRoman romanDigits2.[o] n)
    |> List.fold (+) ""

//second version
let rec mult s n =
    match n with
    | 0 -> ""
    | _ -> s + (mult s (n - 1))

let toRomanII (one, five, ten) n = 
    match n with 
    | 1 | 2 | 3 -> mult one n
    | 4 -> one + five
    | 5 -> five
    | 6 | 7 | 8 -> five + mult one (n - 5)
    | 9 -> one + ten
    |  _  -> "UGH"

let convertArabicToRomanII arabic = 
    let ordinal o = arabic % (10 * o) / o
    [
        yield ordinal 1000 |> toRomanII ("M", "","");
        yield ordinal 100 |> toRomanII ("C", "D", "M");
        yield ordinal 10 |> toRomanII ("X", "L","C");
        yield ordinal 1 |> toRomanII ("I", "V", "X");
    ]
    |> List.fold (+) ""

[1..4000] |> List.map convertArabicToRomanII




