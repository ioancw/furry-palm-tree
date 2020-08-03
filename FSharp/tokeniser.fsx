open System

type Token =
    | N of int
    | C of char

let getToken =
    function
    | C c -> c

let tokenize source =

    let rec number a l =
        match l with
        | w :: t when Char.IsWhiteSpace(w) -> a, t
        | [] -> a, []
        | h :: t when Char.IsDigit(h) -> number ((a * 10) + (Int32.Parse(h.ToString()))) t
        | h :: t -> a, h :: t

    let rec tokenize' a =
        function
        | h :: t when Char.IsLetter h -> tokenize' (C(h) :: a) t
        | h :: t when Char.IsDigit(h) ->
            let n, t' = number (Int32.Parse(h.ToString())) t
            tokenize' (N(n) :: a) t'
        | [] -> a
        | _ -> failwith "Unexpected character."

    tokenize' [] source

let rec mult s (n: int): string =
    match n with
    | 0 -> ""
    | _ -> s + (mult s (n - 1))

let rec expander chars (expanded: string list) =
    match chars with
    | [] -> expanded
    | n :: c :: t ->
        match n with
        | N n ->
            let multiplied = mult (string (getToken c)) (n)
            expander t (multiplied :: expanded)
        | C n -> expander t (string n :: string (getToken c) :: expanded)
    | c :: t -> expander t (string (getToken c) :: expanded)

let x = [ N 12; C 'A'; N 4; C 'B' ]
let y = [ N 12; C 'A' ]

expander x [] |> List.rev |> List.fold (+) ""
