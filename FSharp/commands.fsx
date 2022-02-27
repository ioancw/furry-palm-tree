open System 
open System.Diagnostics
open System.Threading.Tasks

type CommandResult = {
    ExitCode: int;
    StandardOutput: string;
    StandardError: string;
}

let executeCommand executable args = 
    async {
        let startInfo = ProcessStartInfo()
        startInfo.FileName <- executable

        for arg in args do
            startInfo.ArgumentList.Add(arg)

        startInfo.RedirectStandardOutput <- true 
        startInfo.RedirectStandardError <- true 
        startInfo.UseShellExecute <- false 
        startInfo.CreateNoWindow <- true 

        use process = new Process()
        process.StartInfo <- startInfo 
        process.Start() |> ignore 

        let outTask = Task.WhenAll( [|
            process.StandardOutput.ReadToEndAsync();
            process.StandardError.ReadToEndAsync()
        |])

        do! process.WaitForExitAsync() |> Async.AwaitTask 
        let! out = outTask |> Async.AwaitTask
        return {
            ExitCode = process.ExitCode;
            StandardOutput = out.[0];
            StandardError = out.[1];
        }        
    }

let executeCmdCommand command = 
    executeCommand "cmd.exe" [command]

let r = executeCmdCommand "mkdir abc" |> Async.RunSynchronously

if r.ExitCode = 0 then 
    printfn "%s" r.StandardOutput 
else 
    eprintfn "%s" r.StandardError
    Environment.Exit(r.ExitCode)

type StaleType = 
    | Reason1 of string 
    | Reason2 of string

type MyDU = 
    | Fresh 
    | Stale of StaleType 
    | Other 

let processMyDu (mydu: MyDU) = 
    match mydu with 
    | Fresh | Stale _ -> "fresh or stale" 
    | Other -> "other"

let mydu = MyDU.Fresh
let mydu' = MyDU.Other 
let mydu'' = MyDU.Stale(StaleType.Reason1("Reason1"))
let mydu''' = MyDU.Stale(StaleType.Reason2("Reason2"))

processMyDu mydu
processMyDu mydu'
processMyDu mydu''
processMyDu mydu'''



