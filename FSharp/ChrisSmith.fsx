open System.Windows.Forms.VisualStyles.VisualStyleElement.ListView
open System.Buffers.Binary
let generatePowerOfFunc baseValue =
  (fun exponent -> baseValue ** exponent)

let powerOfTwo = generatePowerOfFunc 2.0

powerOfTwo 8.0
let powerOfThree = generatePowerOfFunc 3.0

powerOfThree 2.0

open System.Text.RegularExpressions
let (===) str (regex : string) =
    Regex.Match(str, regex).Success

"The quick brown fox" === "The (.*) fox"
 
 // binary tree
type BinaryTree = 
    | Node of int * BinaryTree * BinaryTree 
    | Empty 

let rec printInOrder tree = 
    match tree with
    | Node (data, left, right)
        -> printInOrder left 
           printfn "Node %d" data
           printInOrder right
    | Empty
        -> ()