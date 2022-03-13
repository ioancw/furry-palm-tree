(*** raw ***)
---
layout: post
title:  "Event Post"
description: "Subcribe to messages posted to a mailbox."
date:   2021-11-28 06:07:00 +0100
categories: 
tags: F# FSharp Literate Formatting
---

(**
My Blog
-------
I use github's Jekyll as a 'blog engine'.  This means I can write posts in markdown 
and commit to the repository at which point the blog engine renders in nice html.
*)
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