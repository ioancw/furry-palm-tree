open System
open System.Collections.Generic

let _deps = ResizeArray<HashSet<int>>()
let mutable _topoSortedDeps : int array array = null
//A node
//B's precedents: A, A's dependents are B and C
//C's precedents: A
//D's precedents: C, C's dependents is D
//E's precedents: B, D
//F's precedents: B, E

let addDep prec dep = 
    _deps.[prec].Add(dep) |> ignore<bool>

let addNode (node: char) (precedents: int[]) = 
    let id = _deps.Count
    _deps.Add(HashSet()) 

    for precId in precedents do
        addDep precId id
    
    id

let aNode = addNode 'A' Array.empty //0
let bNode = addNode 'B' [|aNode|] //1
let cNode = addNode 'C' [|aNode|] //2
let dNode = addNode 'D' [|cNode|] //3
let eNode = addNode 'E' [|bNode;dNode|] //4
let fNode = addNode 'F' [|bNode;eNode|] //5

//toposort, starting from 0 (i.e. A)
//based on DFS
let getTopoSortedDependants (id: int): int[] = 
    if isNull _topoSortedDeps then
        _topoSortedDeps <- Array.zeroCreate _deps.Count

    if isNull _topoSortedDeps.[id] then
        let visited = HashSet<_>()
        let sortOrder = Stack<_>()

        let rec topoVisit startId = 
            if not <| visited.Contains(startId) then
                for depId in _deps.[startId] do
                    topoVisit(depId)

                visited.Add(startId) |> ignore<bool>
                sortOrder.Push(startId)

        for depId in _deps.[id] do
            topoVisit depId

        _topoSortedDeps.[id] <- sortOrder.ToArray()

    _topoSortedDeps.[id]

getTopoSortedDependants aNode

type Node<'a> = {id: int; value: 'a }
