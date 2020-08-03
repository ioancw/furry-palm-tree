// Learn more about F# at http://fsharp.org
open System

type Message =
    | Message1 of string
    | Message2 of string

type State = {Field1: string; Field2: string}

let stateWriter state= 
    printfn "State now: %s, %s" state.Field1 state.Field2

let updater state msg = 
    match msg with
    | Message1 m ->
        printfn "Got Message1: %s" m
        {state with Field1 = m}
    | Message2 m ->
        printfn "Got Message2: %s" m
        {state with Field2 = m}

let stateUpdater initialState updater stateWriter =     
    stateWriter initialState
    let agent = 
        MailboxProcessor<Message>.Start(fun inbox ->
            let rec loop (state: State) = 
                async {
                    let! msg = inbox.Receive()
                    let updatedState = updater state msg
                    do stateWriter updatedState
                    return! loop updatedState
                }
            loop initialState
        )
    agent

//https://www.developerfusion.com/article/139804/an-introduction-to-f-agents/

[<EntryPoint>]
let main argv =
    let updatingAgent = stateUpdater 
                            {Field1 = "Ioan"; Field2 = "Williams"} 
                            updater
                            stateWriter

    updatingAgent.Post(Message1 ("Bob"))
    updatingAgent.Post(Message2 ("Jones"))
    Console.ReadKey(true) |> ignore
    0 // return an integer exit code
