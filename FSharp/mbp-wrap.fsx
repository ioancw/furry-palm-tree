let counter0 =
    MailboxProcessor.Start(fun inbox ->
        let rec loop n =
            async {
                let! msg = inbox.Receive()
                return! loop (n + msg) }

        loop 0)

counter0.Post(1)

open System

// set up a mailbox processor so that it can be spawned
type AfterError<'state> =
    | ContinueProcessing of 'state
    | StopProcessing
    | RestartProcessing

type msg1 =
    | Message1
    | Message2 of int
    | Message3 of string

type MailboxProcessor<'a> with

    static member public SpawnAgent<'b>( messageHandler    : 'a -> 'b -> 'b
                                       , initialState      : 'b
                                       , ?timeout          : 'b -> int
                                       , ?timeoutHandler   : 'b -> AfterError<'b>
                                       , ?errorHandler     : Exception -> 'a option -> 'b -> AfterError<'b>)
                                                           : MailboxProcessor<'a> =
        let timeout = defaultArg timeout (fun _ -> -1)

        let timeoutHandler =
            defaultArg timeoutHandler (fun state -> ContinueProcessing(state))

        let errorHandler =
            defaultArg errorHandler (fun _ _ state -> ContinueProcessing(state))

        MailboxProcessor.Start(fun inbox ->
            let rec loop (state) =
                async {
                    let! msg = inbox.TryReceive(timeout (state))
                    try
                        match msg with
                        | None ->
                            match timeoutHandler state with
                            | ContinueProcessing (newState) -> return! loop (newState)
                            | StopProcessing -> return ()
                            | RestartProcessing -> return! loop (initialState)
                        | Some (m) -> return! loop (messageHandler m state)
                    with ex ->
                        match errorHandler ex msg state with
                        | ContinueProcessing (newState) -> return! loop (newState)
                        | StopProcessing -> return ()
                        | RestartProcessing -> return! loop (initialState)
                }

            loop (initialState))

    static member public SpawnWorker(messageHandler, ?timeout, ?timeoutHandler, ?errorHandler) =
        let timeout = defaultArg timeout (fun () -> -1)

        let timeoutHandler =
            defaultArg timeoutHandler (fun _ -> ContinueProcessing(()))

        let errorHandler =
            defaultArg errorHandler (fun _ _ -> ContinueProcessing(()))

        MailboxProcessor.SpawnAgent
            ((fun msg _ ->
                messageHandler msg
                ()), (), timeout, timeoutHandler, (fun ex msg _ -> errorHandler ex msg))

type msg =
    | Increment of int
    | Fetch of AsyncReplyChannel<int>
    | Stop

exception StopException

type CountingAgent() =

    let handler =
        (fun msg n ->
            match msg with
            | Increment m -> n + m
            | Stop -> raise (StopException)
            | Fetch replyChannel ->
                do replyChannel.Reply(n)
                n)

    let counter =
        MailboxProcessor.SpawnAgent(handler, 0, errorHandler = (fun _ _ _ -> StopProcessing))

    member _.Increment(n) = counter.Post(Increment(n))
    member _.Stop() = counter.Post(Stop)
    member _.Fetch() = counter.PostAndReply(fun replyChannel -> Fetch(replyChannel))

let counter2 = CountingAgent()

counter2.Increment(1)
counter2.Fetch()
counter2.Increment(2)
counter2.Fetch()

counter2.Stop()
//toned down version of an agent/worker
