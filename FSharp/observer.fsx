open System
open System.Threading

/// create a timer and register an event handler,
/// then run the timer for five seconds
let createTimer timerInterval eventHandler =
    // setup a timer
    let timer =
        new System.Timers.Timer(float timerInterval)

    timer.AutoReset <- true

    // add an event handler
    timer.Elapsed.Add eventHandler

    // return an async task
    async {
        // start timer...
        timer.Start()
        // ...run for five seconds...
        do! Async.Sleep 5000
        // ... and stop
        timer.Stop()
    }
///////
// create a handler. The event args are ignored
let basicHandler _ = printfn "tick %A" DateTime.Now

// register the handler
let basicTimer1 = createTimer 1000 basicHandler

// run the task now
Async.RunSynchronously basicTimer1

//observable
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

// create the timer and the corresponding observable
let basicTimer2, timerEventStream = createTimerAndObservable 1000

// register that everytime something happens on the
// event stream, print the time.
timerEventStream
|> Observable.subscribe (fun _ -> printfn "tick %A" DateTime.Now)

// run the task now
Async.RunSynchronously basicTimer2
