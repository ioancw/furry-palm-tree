// Take from chapter 8 of F# Deep Dives, Asynchronous agent-based programming

open System.IO
open System.Net

let doWebRequest (url: string) method transformer =
    async {
        let request = WebRequest.Create(url, Method = method)
        use! response = request.AsyncGetResponse()
        return! transformer (response.GetResponseStream())
    }

let readStreamAsString (stream: Stream) =
    async {
        use streamReader = new StreamReader(stream)
        return! streamReader.ReadToEndAsync() |> Async.AwaitTask
    }

//combine the above functions to download webpage.

let result =
    doWebRequest "http://www.google.com" "GET" readStreamAsString
    |> Async.RunSynchronously

//agents

//In F# the MailboxProcessor is backed by a queue.
//Messages can be posted to the queue asynchronously and then processed in the actor loop.

type Agent<'a> = MailboxProcessor<'a>

type Request = Get of string * AsyncReplyChannel<string>

let downloadAgent =
    Agent<_>.Start(fun inbox ->
        let rec loop (cache: Map<string, string>) =
            async {
                let! msg = inbox.Receive()

                match msg with
                | Get(url, reply) ->
                    match cache.TryFind(url) with
                    | Some(result) ->
                        reply.Reply(result)
                        return! loop cache
                    | None ->
                        let! result = doWebRequest url "GET" readStreamAsString
                        reply.Reply(result)
                        return! loop (Map.add url result cache)
            }

        loop Map.empty

    )

let agentResult =
    downloadAgent.PostAndReply(fun reply -> Get("http://www.google.com", reply))
