type IGraph<'Node> when 'Node : comparison = 
    abstract distanceTo : 'Node -> 'Node -> float
    abstract iterNeighbors : 'Node -> ('Node -> unit) -> unit

[<CustomEquality>]
[<CustomComparison>]
type Meta<'t> when 't : comparison = 
    { path: 't list
      g: float
      f: float
      vertex: 't}

    override x.Equals y = 
        compare x (y :?> Meta<_>) = 0
    override x.GetHashCode() = x.vertex.GetHashCode()

    interface System.IComparable with
        member x.CompareTo y = 
            let y = (y :?> Meta<_>)
            compare (x.f, x.vertex) (y.f, y.vertex)

let (|Empty|First|) xs = 
    if Set.isEmpty xs then Empty else
        let x = Set.minElement xs
        First(x, Set.remove x xs)

let foldOfIter iter f a xs = 
    let a = ref a
    iter xs (fun x -> a := f !a x)
    !a

let search (graph: IGraph<_>) src dst = 
    let rec loop (meta, closedSet, openSet) = 
        match openSet with
        | Empty -> None, closedSet
        | First(x, openSet) ->
            if x.vertex = dst then Some x.path, closedSet else
                let closedSet = Set.add x.vertex closedSet
                let aux (meta, open_set) y = 
                    if Set.contains y closedSet then meta, openSet else
                        let y' = 
                            let g' = x.g + graph.distanceTo x.vertex y
                            { path = y::x.path
                              g = g'
                              f = g' + graph.distanceTo y dst
                              vertex = y}
                        let meta' openSet = 
                            Map.add y y' meta, Set.add y' openSet
                        if Map.containsKey y meta then
                            let y = Map.find y meta
                            if Set.contains y openSet then
                                if y'.g >= y.g then meta, openSet else
                                    meta'(Set.remove y openSet)
                            else
                                meta' openSet
                        else
                            meta' openSet
                let meta, openSet = foldOfIter graph.iterNeighbors aux (meta, openSet) x.vertex
                loop(meta, closedSet, openSet)
    let x = { path = [src]; g = 0.0; f = graph.distanceTo src dst; vertex = src}
    loop(Map.add src x Map.empty, Set.empty, Set.singleton x)