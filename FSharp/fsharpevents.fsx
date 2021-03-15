open System
open System.Collections.Generic

type EventClass() =

    let event = new Event<_>()

    member this.TestEvent(arg) = event.Trigger(this, arg)

    member this.Event = event.Publish

let classWithEvent = EventClass()

classWithEvent.Event.Add(fun (sender, arg) -> printfn "Event1 occurred! Object data: %A" arg)

let z =
    classWithEvent.Event
    |> Observable.subscribe (fun (sender, arg) -> printfn "Event occurred! Message: %A" arg)

classWithEvent.TestEvent("Hello World!")

/// Counter with F#-only event
type SimpleCounter() =
    let evt = new Event<int>()
    let mutable count = 0

    /// Increments the counter and triggers
    /// event after every 10 increments
    member x.Increment() =
        count <- count + 1
        if count % 10 = 0 then evt.Trigger(count)

    member x.RollOneHunderd() =
        [ 1 .. 100 ]
        |> List.iter (fun cnt -> x.Increment())

    /// Event triggered after every 10 increments
    /// The value carried by the event is 'int'
    member x.IncrementedTenTimes = evt.Publish

let simpleCounter = SimpleCounter()

let cntr =
    simpleCounter.IncrementedTenTimes
    |> Observable.subscribe (fun count -> printfn "Count is: %A" count)

simpleCounter.RollOneHunderd()

let createTimerAndObservable timerInterval =
    // setup a timer
    let timer =
        new System.Timers.Timer(float timerInterval)

    timer.AutoReset <- true

    // events are automatically IObservable
    let observable = timer.Elapsed

    // return an async task
    let task =
        async {
            timer.Start()
            do! Async.Sleep 5000
            timer.Stop()
        }

    // return a async task and the observable
    (task, observable)

let timerCount2, timerEventStream = createTimerAndObservable 500

// set up the transformations on the event stream
timerEventStream
|> Observable.scan (fun count _ -> count + 1) 0
|> Observable.subscribe (fun count -> printfn "timer ticked with count %i" count)

// run the task now
Async.RunSynchronously timerCount2
