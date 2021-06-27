//
[<Measure>]
type percent

type MotorInput =
    { ClockwiseDutyCycle: float<percent>
      CounterClockwiseDutyCycle: float<percent> }
    member t.DirectionDutyCycle =
        t.ClockwiseDutyCycle - t.CounterClockwiseDutyCycle

let makeMotorInput (cw: float<percent>) (ccw: float<percent>) =
    { ClockwiseDutyCycle = max (min cw 100.0<percent>) 0.0<percent>
      CounterClockwiseDutyCycle = max (min ccw 100.0<percent>) 0.0<percent> }

let makeDirectionalMotorInput (directionalDutyCycle: float<percent>) =
    if directionalDutyCycle >= 0.0<percent> then
        makeMotorInput directionalDutyCycle 0.0<percent>
    else
        makeMotorInput 0.0<percent> (-directionalDutyCycle)

[ makeDirectionalMotorInput 101.0<percent>
  makeDirectionalMotorInput 50.0<percent>
  makeDirectionalMotorInput 0.0<percent>
  makeDirectionalMotorInput -101.0<percent> ]

[<Measure>]
type rpm

type MotorOutput = float<rpm>

type ControlError = float<rpm>

let controlWithProportionalResponse (proportion: float<percent / rpm>) (error: ControlError) = proportion * error

type ErrorIntegral =
    { MaxErrors: int
      Errors: ControlError list }
    member t.TotalError = List.sum t.Errors
    member t.NumErrors = t.Errors.Length

    member t.LastError =
        match t.Errors with
        | x :: _ -> x
        | _ -> 0.0<rpm>

    member t.Add error =
        { t with
              Errors =
                  let n = t.Errors.Length

                  if n < t.MaxErrors then
                      error :: t.Errors
                  else
                      error :: (t.Errors.[..(n - 2)]) }

let makeErrorIntegral (maxErrors: int) = { MaxErrors = maxErrors; Errors = [] }

let ei = makeErrorIntegral 2
ei

let ei3 =
    ei.Add(100.0<rpm>).Add(200.0<rpm>).Add(300.0<rpm>)

[ 100.0<rpm>; 200.0<rpm>; 300.0<rpm> ]
|> List.iter (ei.Add >> ignore)

let controlWithIntegralResponse (proportion : float<percent/rpm>)
                                (errorIntegral : ErrorIntegral) = 
    proportion * errorIntegral.TotalError
