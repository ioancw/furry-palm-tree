// standard recursive, not tail recursive.
let rec factorial n  = 
    match n with 
    | 0 -> 1
    | _ -> n * factorial (n - 1)

factorial 5

// tail recursive, with an accumulator
let factorialAcc n = 
    let rec factorial n' acc = 
        match n' with 
        | 0 | 1 -> 1
        | _ -> factorial (n - 1) (acc * n')
    factorial n 1

factorialAcc 5

// uses a fold
let factorialFold n = [1 .. n] |> List.fold (fun s n -> s * n) 1
factorialFold 5
// can cancel out folder function to only have the operator
let factorialFold' n = [1 .. n] |> List.fold ( * ) 1
factorialFold' 5 

// reduce 
let factorialReduce n = [1 .. n] |> List.reduce (fun s n -> s * n)
factorialReduce 5
let factorialReduce' n = [1 .. n] |> List.reduce ( * )
factorialReduce' 5