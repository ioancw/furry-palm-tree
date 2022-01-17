let collect x =
    function
    | (y :: xs) :: xss when x = y -> (x :: y :: xs) :: xss //add to start of existing list.
    | xss -> [ x ] :: xss //create a new list

List.foldBack collect ("aaaabbbcca" |> List.ofSeq) []
|> List.map (List.countBy id)


type Prop = 
    | Variable of string
    | And of Prop * Prop
    | Or of Prop * Prop
    | Negative of Prop 

let rec displayProp prec prop = 
    let useBracket lim s = 
        if prec < lim then "(" + s + ")" else s
    match prop with 
    | Or (p1, p2) -> useBracket 8 (displayProp 7 p1) + "|" + (displayProp 6 p2)
    | And (p1, p2) -> useBracket 3 (displayProp 6 p1) + "&" + (displayProp 5 p2)
    | Negative p -> useBracket 2 ("!" + displayProp 1 p)
    | Variable v -> v

displayProp 6 (Negative(Variable "x"))

displayProp 6 (And(Variable "x", Variable "y"))

displayProp 10 (And(Variable "x", And(Variable "y", Variable "z")))