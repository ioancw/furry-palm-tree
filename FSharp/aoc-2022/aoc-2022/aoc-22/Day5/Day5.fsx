open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic

let fileName = "day5.txt"

let data =
    Path.Combine(__SOURCE_DIRECTORY__, fileName)
    |> File.ReadAllLines
    |> List.ofArray
    
let testData =
    [
        "    [D]           "
        "[N] [C]           "
        "[Z] [M] [P]       "
        " 1   2   3        "
        ""
        "move 1 from 2 to 1"
        "move 3 from 1 to 3"
        "move 2 from 2 to 1"
        "move 1 from 1 to 2"
    ]

type Instruction = {numberToMove: int; fromStack: int; toStack: int}

module Stack = 
    
    let createInitialStackState count initialState =
        let stacks = [for _ in 1 .. count -> Stack<char>()]
        let push stackNumber value = stacks[stackNumber - 1].Push(value)
        
        initialState
        |> List.map (fun (stackNumber, value) -> push stackNumber value)
        |> ignore
        
        stacks
        
    let move (stacks: Stack<char> list) (instruction: Instruction) =
        for _ in 1 .. instruction.numberToMove do
            let element = stacks[instruction.fromStack - 1].Pop()
            stacks[instruction.toStack - 1].Push(element)
        stacks
        
    let moveMany (stacks: Stack<char> list) (instruction: Instruction) =
        [for _ in 1 .. instruction.numberToMove -> stacks[instruction.fromStack - 1].Pop()]//popped in reverse order
        |> List.rev
        |> List.iter (fun i -> stacks[instruction.toStack - 1].Push(i))
        stacks
        
    let moveStackElements f (stacks: Stack<char> list) (instructions: Instruction list) =
        instructions |> List.fold f stacks
        
    let getTopOfStacks (stacks: Stack<char> list) = 
        stacks
        |> List.map (fun s -> s.Peek() |> string)
        |> List.fold (+) "" 
 
let parseStackData data =  
    
    let splitIndex = 
        data
        |> List.findIndex Seq.isEmpty
        
    let state, instructions =
        data
        |> List.splitAt splitIndex
        |> fun (state, instruction) ->
            state |> List.rev,
            instruction |> List.tail
           
    let parseNumberStackRow row =
        row
        |> Seq.filter(fun c -> c <> ' ')
        |> Seq.map (fun c -> int c - int '0')
        |> Seq.max
        
    let parseStackRow row =
        row
        |> Seq.chunkBySize 4 // splits the row into 4 equal chunks, chunk 1 to stack 1, chunk 2 stack 2 etc.
        |> Seq.mapi (fun i c -> i + 1, c[1]) // each chunk is of the form "[Z] ". So take 2nd character of each chunk, along with number
        |> Seq.filter (fun p -> snd p <> ' ')
                   
    let parseInstructionRow row =
        let instructionPattern = "move ([0-9]+) from ([0-9]+) to ([0-9]+)"
        let isMatch = Regex.Match(row, instructionPattern)
        {
          numberToMove = int isMatch.Groups[1].Value
          fromStack = int isMatch.Groups[2].Value
          toStack = int isMatch.Groups[3].Value
        }
        
    let parsedInstructions =
        instructions |> List.map parseInstructionRow

    let numberOfStacks = 
        state
        |> List.head
        |> parseNumberStackRow    

    let parsedInitialStack =
        state
        |> List.tail
        |> List.map parseStackRow
        |> Seq.collect id
        |> Seq.toList
        
    let initialStack = Stack.createInitialStackState numberOfStacks parsedInitialStack
    initialStack, parsedInstructions
    
//test    
//let initialStack, instructions = parseStackData testData    
//Stack.moveStackElements initialStack instructions |> Stack.getTopOfStacks
//Part 1
let initialStack, instructions = parseStackData data
Stack.moveStackElements Stack.move initialStack instructions |> Stack.getTopOfStacks
//Part2
Stack.moveStackElements Stack.moveMany initialStack instructions |> Stack.getTopOfStacks
initialStack
instructions