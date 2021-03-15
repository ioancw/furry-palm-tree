let collect x =
    function
    | (y :: xs) :: xss when x = y -> (x :: y :: xs) :: xss //add to start of existing list.
    | xss -> [ x ] :: xss //create a new list

List.foldBack collect ("aaaabbbcca" |> List.ofSeq) []
|> Seq.map (Seq.countBy id)
