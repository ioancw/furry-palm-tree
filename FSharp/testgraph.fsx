//identifier * vertex data
type VertexData<'V> = int * 'V

//identifier * priority * vertex target * edge data
type EdgeData<'E> = int * int * int * 'E

type Adjacency<'E> = EdgeData<'E> list

type Vertex<'V, 'E> = VertexData<'V> * Adjacency<'E>

//graph is a list of verticies
type Graph<'V, 'E> =  int * Vertex<'V, 'E> list

let empty: Graph<_,_> = (0, [])

//helper functions to get data from a vertex
let vertexId (v: Vertex<_,_>) = v |> fst |> fst
let vertexData (v: Vertex<_,_>) = v |> fst |> snd

//helper functions to get data from an edge
let edgeId ((x, _, _, _): EdgeData<_>) = x 
let edgePriority ((_, x, _, _): EdgeData<_>) = x 
let edgeTarget ((_, _, x, _): EdgeData<_>) = x 
let edgeData ((_, _, _, x): EdgeData<_>) = x 

let getVertex v (g: Graph<_,_>) : Vertex<_,_> = snd g |> List.find (fun V -> vertexId V = v)
let getEdges v (g: Graph<_,_>) = g |> getVertex v |> snd

//add to graph
let addVertex (v: 'V) (g: Graph<'V,_>) : (int * Graph<'V, _>) = 
    let id = fst g 
    let s = snd g  
    let newVD : VertexData<_> = (id, v)
    let newA : Adjacency<_> = []
    let newV = (newVD, newA)
    (id, (id + 1, newV::s))

let addEdge priority (v: int) (v': int) (e: 'E) (g: Graph<'V, 'E>) : (int * Graph<'V, 'E>) = 
    let id = fst g 
    let s = snd g
    let newE : EdgeData<_> = (id, priority, v', e)
    (id, 
        (id + 1, 
            s|> List.map (fun V ->
                if (vertexId V) = v then
                    (fst V, newE::(snd V))
                else V)))

let sortEdges (a: Adjacency<_>) = 
    a |> List.sortBy edgePriority       

let removeEdge (id: int) (g: Graph<_,_>) : Graph<_,_> = 
    let next = fst g  
    let s = snd g
    (next, s |> List.map (fun (v, a) -> (v, a |> List.filter (fun x -> (edgeId x) <> id ))))

let removeVertex (id: int) (g: Graph<_,_>) : Graph<_,_> = 
    let next = fst g 
    let s = snd g  
    (next, s |> ([] |> List.fold (fun s' (v, a) -> 
                        if (fst v) = id then s'
                        else
                            let f = fun x -> ((edgeTarget x) <> id)
                            let newA = a |> List.filter f 
                            let newV = (v, newA)
                            newV::s')))

//test the graph out

empty
|> addVertex "vertex1"
|> (fun (v1, g) -> 
    (addVertex "vertex2" g)
    |> (fun (v2, g) -> 
        (addEdge 0 v1 v2 "edge1" g)))
|> snd
|> printfn "Parsed graph: %A"                
