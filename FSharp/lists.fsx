//This sums a list using the fold method and using the operator in a bracket () means it's treated as a function
let sum list = List.fold (+) 0 list

let sumBetter (l : int list) = List.sum l

//find remove duplicates from a list
let removeDuplicates list = 
    list
    |> Seq.distinct
    |> Seq.toList

let rec printList list = 
    match list with
    | [] -> ()
    | head :: tail ->
        printf "%d" head
        printList tail
        
let convertIntListToInt (list : int list ) = 
    list
    |> List.map (fun i -> i.ToString())
    |> String.concat ""
    |> int

let convertIntListToInt2 (l : int list) = 
    l
    |> List.fold (fun acc x -> acc + x.ToString()) ""
    |> int

let rec convertToString (l) = 

    match l with
    | [] -> ""
    | head :: tail -> head.ToString() + convertToString tail

let isEmptySet (set : Set<'a>) = set.Count = 0

let isPermutation l1 l2 = 
    Set.difference (Set.ofList l1) (Set.ofList l2)
    |> isEmptySet

let wordlist = ["dog";"god";"rod"]

//Anagrams.
let sortString (s:string) = s |> Seq.sort |> Seq.toList |> convertToString

let anagramKey (s: string) = sortString s

let isAnagram (s1:string) (s2:string) anagramKey = 
    anagramKey s1 = anagramKey s2

wordlist |> Seq.iter (fun x -> printfn "%s" x)