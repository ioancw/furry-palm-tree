//turtle
open System
//common
type Distance = float

type [<Measure>] Degrees 

type Angle = float<Degrees>

type PenState = Up | Down

type PenColor = Black | Red | Blue 

type Position = {x: float; y: float}

let round2 (flt: float) = Math.Round(flt, 2)

let calcNewPostition (distance: Distance) (angle: Angle) currentPosition = 
    let angleInRadians = angle * (Math.PI/180.0) * 1.0<1/Degrees>
    let x0 = currentPosition.x 
    let y0 = currentPosition.y 
    let x1 = x0 + (distance * Math.Cos angleInRadians)
    let y1 = y0 + (distance * Math.Sin angleInRadians)
    {x = round2 x1; y = round2 y1}

let initialPosition, initialColor, initialPenState = 
    {x = 0.; y = 0.}, Black, Down

let dummyDrawLine log oldPos newPos color = 
    log (sprintf "...Draw line from (%0.1f, %0.1f) to (%0.1f, %0.1f) using %A"
        oldPos.x oldPos.y newPos.x newPos.y color)

//Basic OO implementation

type Temp = F of float | C of float

module Temperature = 
    let fold fFunction cFunction = function
        | F f -> fFunction f 
        | C c -> cFunction c 

let fFever temp = 
    if temp > 100.0 then "Fever!" else "Ok"

let cFever temp = 
    if temp > 38.0 then "Fever!" else "Ok"

let isFever = Temperature.fold fFever cFever

let nTemp = C 37.0
let r1 = isFever nTemp

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

myfunc 6 4 |> Seq.iter (printfn "%A")