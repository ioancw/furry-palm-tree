// topological sort
let graph = 
    //vertex, edge
    [
        5, 11
        11, 2
        11, 10
        11, 9
        7, 11
        7, 8
        8, 9
        3, 10
        3, 8
    ]

let allVerticies graph = 
    seq {for (v, e) in graph do yield v; yield e}
    |> Set.ofSeq

let rec removeVerticies edges = 
    match edges with
    | [] -> []
    | _ -> 
        let outgoing, incoming = List.unzip edges
        let outgoingSet = outgoing |> Set.ofList
        let incomingSet = incoming |> Set.ofList
        let allVerticies = Set.union outgoingSet incomingSet 
        let verticiesNoIncoming = 
            Set.difference allVerticies incomingSet
            |> Set.toList
        let remainingEdges = List.filter (fun (v, _) -> not <| List.contains v verticiesNoIncoming) edges
        verticiesNoIncoming @ (removeVerticies remainingEdges)

let topoSort graph = 
    let verticies = allVerticies graph 
    let remove = removeVerticies graph 
    let isolated = Set.difference verticies (remove |> Set.ofList) |> Set.toList
    List.append remove isolated

topoSort graph