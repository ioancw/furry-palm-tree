#r "nuget: Elmish"

open Elmish
open System

module Program =
    let mkProgramWithOrderExecute
        (init: 'arg -> 'model * 'order)
        (update: 'msg -> 'model -> 'model * 'order)
        (view: 'model -> Dispatch<'msg> -> 'view)
        (execute: 'order -> Dispatch<'msg> -> unit)
        =
        let convert (model, order) = model, order |> execute |> Cmd.ofSub
        Program.mkProgram (init >> convert) (fun msg model -> update msg model |> convert) view


let description = "count-o-matic
Press Space to start, pause or resume
Press Enter to step while in pause.
Press +/- to increase/decrease speed while running.
Press Q to quit"

type Model =
    { Running: bool
      Count: int
      Interval: int }

type Msg =
    | TimerTick
    | KeyboardTick
    | Toggle
    | ChangeInterval of offset: int

type Order =
    | StartKeyListener
    | Print of value: int
    | DelayTick of delay: int
    | CancelDelayedTick
    | Orders of Order list
    | NoOrder

let init (running, interval) =
    let model =
        { Running = running
          Count = 0
          Interval = interval }

    model,
    Orders [ StartKeyListener
             if running then DelayTick 0 ]

let update msg model =
    match msg, model.Running with
    | TimerTick, true
    | Toggle, false ->
        let model' =
            { model with
                  Running = true
                  Count = model.Count + 1 }

        model',
        Orders [ Print(model.Count + 1)
                 DelayTick model.Interval ]
    | Toggle, true -> { model with Running = false }, CancelDelayedTick
    | KeyboardTick, false -> { model with Count = model.Count + 1 }, Print(model.Count + 1)
    | ChangeInterval x, true ->
        { model with
              Interval = model.Interval + x |> min 2500 |> max 50 },
        NoOrder
    | KeyboardTick, true
    | ChangeInterval _, false
    | TimerTick, false -> model, NoOrder

let view _ _ = ()

let rec execute order dispatch =
    match order with
    | StartKeyListener ->
        async {
            seq {
                while true do
                    (Console.ReadKey true).KeyChar
            }
            |> Seq.takeWhile (fun key -> key <> 'q' && key <> 'Q')
            |> Seq.iter
                (function
                | ' ' -> dispatch Toggle
                | '\013' -> dispatch KeyboardTick
                | '-' ->
                    Console.WriteLine "minus"
                    dispatch (ChangeInterval 50)
                | '+' -> dispatch (ChangeInterval -50)
                | _ -> ())

            Async.CancelDefaultToken()
        }
        |> Async.StartImmediate
    | Print value -> Console.Write value  
    | DelayTick delay ->
        async {
            do! Async.Sleep delay
            dispatch TimerTick
        }
        |> Async.Start
    | CancelDelayedTick -> Async.CancelDefaultToken()
    | Orders orders ->
        for order in orders do
            execute order dispatch
    | NoOrder -> ()

Console.WriteLine description

Program.mkProgramWithOrderExecute init update view execute
|> Program.runWith (false, 350)
