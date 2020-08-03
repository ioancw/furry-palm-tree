type Operator = 
    | Add
    | Sub
    | Mul
    | Div

type Operand = 
    | Integer of int 

type Expression = 
    | OperatorExpression of Operator
    | OperandExpression of Operand 

type Equation = 
    | Equation of Expression list

type EquationError = 
    | Empty
    | Invalid of stirng
    | Unbalanced

// Result<_, > is smarter Option<_>
// as it allows us to pass a failure value
let parseExpression (word: string): Result<Expression, EquationError> = 
    match word with
    | "+" -> Ok(OperatorExpression Add)
    | "-" -> Ok(OperatorExpression Sub)
    | "*" -> Ok(OperatorExpression Div)
    | Int32 i -> Ok(OperandExpression(Integer i))
    | _ -> Error(Invalid word)


type mine =
    {first: string
     second: string
     third: string}

     static member firsty = 
        (fun x -> x.first), (fun f x -> {x with first = f})
     
     static member secondy = 
        (fun x -> x. second), (fun s x -> {x with second = s})

let myTest1 = {first = "Ioan"; second = "Ceredig"; third = "Williams"}    

//now change the second.

let myTest2 = 
    let setter = mine.secondy |> snd 
    setter "Danger" myTest1

let changeSomething 
        (getter: mine -> string)
        (setter: string -> mine -> mine)
        (old: mine) 
        (newOne: mine) : mine = 
        let oldValue = getter old
        let newValue = getter newOne
        if oldValue <> newValue then
            setter oldValue newOne
        else        
            oldOne

let newOne = changeSomething
                (mine.firsty |> fst)
                (mine.firsty |> snd) 
                myTest1
                myTest2
