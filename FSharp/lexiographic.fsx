// lexiographic sort
type myOrder = 
    | One 
    | Two 
    | Three 
    | Four

let myOrderList = [Four;Three;Two;One]

let sortedMyOrderList = myOrderList |> List.sort

//or sort using custom compare

let myOrderList' = ["Four"; "Three"; "One"; "Two"]

let myComparer left right = 
    let number = function 
    | "Four" -> 4
    | "Three" -> 3
    | "Two" -> 2
    | "One" -> 1
    compare (left |> number) (right |> number)

let sorted = myOrderList' |> List.sortWith myComparer 

let myfunc (n: int) (k: int) : seq<int[]> = 
    let rec next (acc: int[]) i = seq{
        if i = k then 
            yield acc 
        else 
            let lb = 
                if i = 0 then 0 else acc.[i - 1] + 1
            for j = lb to n - 1 do 
                acc.[i] <- j
                yield! next acc (i + 1)
    }
    let acc = Array.zeroCreate k
    next acc 0

