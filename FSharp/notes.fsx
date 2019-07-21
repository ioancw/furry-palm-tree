// notes from F# books by chris smith.

// delegates

let functionValue x y = 
    printfn "x = %d, y = %d" x y 
    x + y

type DelegateType = delegate of int * int -> int

let delegateValue1 = 
    new DelegateType(
        fun x y ->
            printfn "x = %d, y = %d" x y
            x + y 
    )

let functionResult = functionValue 1 2
let delegateResult = delegateValue1.Invoke(1,2)

// events

type SetAction = Added | Removed

type SetOperationEventArgs<'a>(value: 'a, action: SetAction) = 
    inherit System.EventArgs()

    member this.Action = action
    member this.Value = value

type SetOperationDelegate<'a> = delegate of obj * SetOperationEventArgs<'a> -> unit

type NoisySet<'a when 'a: comparison>() = 
    let mutable _set = Set.empty: Set<'a>
    let _itemAdded = new Event<SetOperationDelegate<'a>, SetOperationEventArgs<'a>>()
    let _itemRemoved = new Event<SetOperationDelegate<'a>, SetOperationEventArgs<'a>>()

    member this.Add(x) = 
        _set <- _set.Add(x)
        _itemAdded.Trigger(this, new SetOperationEventArgs<_>(x, Added))

    member this.Remove(x) = 
        _set <- _set.Remove(x)
        _itemRemoved.Trigger(this, new SetOperationEventArgs<_>(x, Removed))

    member this.ItemAddedEvent = _itemAdded.Publish

    member this.ItemRemovedEvent = _itemRemoved.Publish

// using the events
let s = new NoisySet<int>()

let setOperationHandler = 
    new SetOperationDelegate<int>(
        fun sender args ->
            printfn "%d was %A" args.Value args.Action
    )

s.ItemAddedEvent.AddHandler(setOperationHandler)
s.ItemRemovedEvent.AddHandler(setOperationHandler)

s.Add(9)

s.Remove(9)

// observable module

