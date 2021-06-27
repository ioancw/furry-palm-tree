
open System

type ISubscription = 
    abstract DataStream: IObservable<string>
    abstract Post: string -> unit

type Client() = 
    member t.MakeSubscription(who: String) : ISubscription = 

        let dataChangedEvent = Event<_>()
        let dataStream = dataChangedEvent.Publish 

        let worker = 
            MailboxProcessor.Start(fun inbox ->
                let rec loop() =
                    async{
                        let! x = inbox.Receive()
                        let dataTimeStamp = DateTime.Now 
                        let returnMessage = sprintf "%s says %s" who x
                        dataChangedEvent.Trigger(returnMessage)
                        ()    
                    }
                loop()    
                )
             
        { new ISubscription with
            member __.DataStream = dataStream :> IObservable<string>
            member __.Post(message: string) = worker.Post message}

let client = Client()
let subs = client.MakeSubscription("me")
subs.DataStream |> Observable.subscribe (fun x -> printfn "%s" x)

subs.Post "hello"

open System
let input = 1.5
let inputS = input.ToString()
        
let parsed = inputS |> Double.TryParse        
