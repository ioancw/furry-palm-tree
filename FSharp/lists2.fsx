type Op =
    | Multiply
    | Add

type Token =
    | Number of int
    | Operator of Op

let test =
    [ Number(1)
      Operator Multiply
      Number(3)
      Operator Add
      Number(1) ]

let result f o s =
    match o with
    | Add -> f + s
    | Multiply -> f * s

let rec eval ls =
    match ls with
    | [ _ ] -> ls
    | Number f :: Operator o :: Number s :: t -> eval (Number(result f o s) :: t)
    | _ when (ls.Length % 2) = 0 -> failwith "Not a balanced list"
    | _ -> failwith (sprintf "Error: %A" ls)

eval test

//test 2
type QOp =
    | And
    | Or

type QToken =
    | Tokens of Set<string>
    | QueryOperator of QOp

let test2 =
    [ Tokens([ "a"; "b"; "c"; "d" ] |> Set.ofList)
      QueryOperator And
      Tokens([ "a"; "b"; "c"; "e" ] |> Set.ofList) ]

let result2 l o r = 
    match o with
    | And -> Set.intersect l r 
    | Or -> Set.union l r 

let rec eval2 ls =
    match ls with
    | [ _ ] -> ls
    | Tokens f :: QueryOperator q :: Tokens s :: t -> eval2 (Tokens(result2 f q s) :: t)
    | _ when (ls.Length % 2) = 0 -> failwith "Not a balanced list"
    | _ -> failwith (sprintf "Error: %A" ls)

eval2 test2
