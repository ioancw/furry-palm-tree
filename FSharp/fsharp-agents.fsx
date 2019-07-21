// queue based system for asynchronously routing messages to handlers using shared memory
// useful when multiple sources/clients need to request something from a single target/server

// mailbox processors are referred to as agents, alias MailboxProcessor as Agent

type Agent<'T> = MailboxProcessor<'T>

// agent that prints whatever is sent to it.

type Message = 
    | Message of obj

let echoAgent = 
    Agent<Message>.Start(
        fun inbox ->
            let rec loop () = 
                async {
                    let! (Message(content)) = inbox.Receive()
                    printfn "%O" content 
                    return! loop()}
            loop()
    )

Message "Test1" |> echoAgent.Post

["a";"b";"b";"c"]
|> List.iter (fun msg -> Message msg |> echoAgent.Post) 

//