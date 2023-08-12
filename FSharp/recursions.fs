//naive recursion in f# to add one to every element in a list.
let rec addOne input = 
    match input with 
    | [] -> []
    | x :: xs -> (x + 1) :: addOne xs 

//but this causes a stack overflow problem
[ 0 .. 40000] |> addOne 

//fix it using tail recursion, using two methods.

//use an accumulator

let outerAddOne input = 
    let rec innerAddOne input accumulator = 
        match input with 
        | [] -> acc 
        | x :: xs -> innerAddOne xs ((x + 1) :: acc)
    innerAddOne input [] |> List.rev
// now no stack overflow, as problem has gone from being a stack problem to a heap problem (unlimited size)
// on each iteration of the function, the call stack is not growing, the heap stores the accumulator, which can grow large.

//continuation passing style - used when the accumulator method doesn't work, as you need to call the recursive function
//more than once in order to get a result, e.g. with trees.

type 'a Tree = 
    | Leaf of 'a 
    | Node of 'a Tree * 'a Tree

//not a tail recursive function that finds the maximum value in a tree.
let rec findMax tree = 
    match tree with 
    | Leaf i -> i 
    | Node (l, r) -> System.Math.Max(findMax l, findMax r)

//can't add an accumulator here, as it requires that findMax be the last call in the function AND called only once.
//but here it is called twice.

//solution is to use continuation passing style.
//similar to using and accumulator, but rather than accumulating a simple object, a function is accumulated.

let findMax2 tree = 
    let rec findMaxInner (tree: int Tree) (continuation: int -> int) = 
        match tree with 
        | Leaf -> i |> continuation 
        | Node (left, right) -> 
            findMaxInner left (fun lMax -> 
                findMaxInner right (fun rMax -> 
                    System.Math.Max(lMax, rMax) |> continuation))

    findMaxInner tree id
