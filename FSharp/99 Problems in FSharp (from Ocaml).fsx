open System.Collections.Generic
//99 Problems in Ocaml

//1. last element of list

let rec last my_list =
    match my_list with
    | [] -> None
    | [ x ] -> Some x
    | h :: tail -> last tail

let rec last2 =
    function
    | [] -> None
    | [ x ] -> Some x
    | h :: tail -> last2 tail

printfn "%A" (last [ 1; 2; 3; 4; 5; 6 ])
printfn "%A" (last2 [ 1; 2; 3; 4; 5; 6 ])

//2. Find last but one element of list

let rec lastTwo =
    function
    | [] -> None
    | [ x; y ] -> Some(x, y)
    | h :: tail -> lastTwo tail

printfn "%A" (lastTwo [ 1; 2; 3; 4; 5; 6 ])

//3. Find kth element of a list

let rec kThElement k (inputList: 'a list) =
    match inputList with
    | [] -> None
    | h :: tail -> if k = 1 then Some h else kThElement (k - 1) tail

printfn "%A" (kThElement 4 [ "A"; "B"; "C"; "D"; "E"; "F"; "G"; "H" ])

//4. Find the number of elements in a list
let rec numberInList =
    function
    | [] -> 0
    | [ _ ] -> 1
    | h :: t -> 1 + numberInList t

//with tail recursion (i.e. using an accumulator)
//So that stackoverflow doesn't occur
let listCount myList =
    let rec countList myList n =
        match myList with
        | [] -> 0
        | [ _ ] -> 1
        | h :: t -> countList t n + 1

    countList myList 0

printfn "%A" (listCount [ "B"; "C"; "D"; "E"; "F" ])

//5. reverse list (not using library function)

open System.Collections
open System

let reverseList (myList: 'a list) =
    myList |> List.fold (fun a x -> x :: a) []

printfn "%A" (reverseList [ 1; 2; 3; 4; 5 ])

//6. find out if list is own palindrome
let isPalindrome myList = myList = reverseList myList

isPalindrome [ 1; 2; 3; 2; 1 ]

//7. flatten a nested list structure
type 'a node =
    | One of 'a
    | Many of 'a node list

let flatten list =
    let rec aux acc =
        function
        | [] -> acc
        | One x :: t -> aux (x :: acc) t
        | Many l :: t -> aux (aux acc l) t in

    List.rev (aux [] list)

flatten [ One "a"; Many [ One "b"; Many [ One "c"; One "d" ]; One "e" ] ]

//8. eliminate consequtive duplicates of list elements

let rec compress =
    function
    | [] -> []
    | h :: t -> if h = List.head t then compress t else h :: compress t


let rec compress2 =
    function
    | a :: (b :: _ as t) -> if a = b then compress2 t else a :: compress2 t
    | smaller -> smaller

let x =
    compress [ "a"; "a"; "a"; "a"; "b"; "c"; "c"; "a"; "a"; "d"; "e"; "e"; "e"; "e" ]

//9.
let pack list =
    let rec aux current acc =
        function
        | [] -> []
        | [ x ] -> (x :: current) :: acc
        | a :: (b :: _ as t) ->
            if a = b then
                aux (a :: current) acc t
            else
                aux [] ((a :: current) :: acc) t in

    List.rev (aux [] [] list)

let x =
    pack [ "a"; "a"; "a"; "a"; "b"; "c"; "c"; "a"; "a"; "d"; "e"; "e"; "e"; "e" ]

let pack2 xs =
    let collect x =
        function
        | (y :: xs) :: xss when x = y -> (x :: y :: xs) :: xss
        | xss -> [ x ] :: xss

    List.foldBack collect xs []

pack2 [ "a"; "a"; "a"; "a"; "b"; "c"; "c"; "a"; "a"; "d"; "e"; "e"; "e"; "e" ]

//10. run length encoding of a list
let encode xs =
    pack2 xs |> List.map (fun l -> (l.Length, l.Head))

encode [ "a"; "a"; "a"; "a"; "b"; "c"; "c"; "a"; "a"; "d"; "e"; "e"; "e"; "e" ]

//11. modified run-length

//need a custom type to hold values of list of single
type 'a Encoding =
    | Multiple of int * 'a
    | Single of 'a

let encode21 xs =
    pack2 xs
    |> List.map (
        (fun l -> (l.Length, l.Head))
        >> (fun (a, b) ->
            match (a, b) with
            | (1, _) -> Single b
            | (a, b) -> Multiple(a, b))
    )

let encode2 xs =
    pack2 xs
    |> List.map (fun l -> (l.Length, l.Head))
    |> List.map (fun (a, b) -> if a > 1 then Multiple(a, b) else Single b)

encode21 [ "a"; "a"; "a"; "a"; "b"; "c"; "c"; "a"; "a"; "d"; "e"; "e"; "e"; "e" ]

//12. Decode a run length encoded list
let decode xs =
    let expand =
        function
        | Multiple(n, x) -> List.replicate n x
        | Single x -> [ x ]

    xs |> List.collect expand

let encoded: 'a Encoding list =
    encode21 [ "a"; "a"; "a"; "a"; "b"; "c"; "c"; "a"; "a"; "d"; "e"; "e"; "e"; "e" ]

decode encoded

//13.

//14 duplicate elements of list
//not tail recursive
let rec duplicate =
    function
    | [] -> []
    | h :: t -> h :: h :: duplicate t

duplicate [ "a"; "b"; "c"; "c"; "d" ]

//15. replicate items of list n times

let replicate xs n =
    let rec rep =
        function
        | [] -> []
        | h :: t -> [ for i in 1..n -> h ] @ rep t

    rep xs

replicate [ "a"; "b"; "c" ] 3

[ for i in 1..3 -> "a" ] @ [ for x in 1..3 -> "b" ]

//16. Drop every Nth element in the list

let rec drop (inputList: 'a list) k =
    match inputList with
    | [] -> None
    | h :: tail -> if k = 1 then Some h else kThElement (k - 1) tail

let drop2 list n =
    let rec aux i =
        function
        | [] -> []
        | h :: t -> if i = n then aux 1 t else h :: aux (i + 1) t

    aux 1 list

drop2 [ "a"; "b"; "c"; "d"; "e"; "f"; "g"; "h"; "i"; "j" ] 3
//string list = ["a"; "b"; "d"; "e"; "g"; "h"; "j"]
