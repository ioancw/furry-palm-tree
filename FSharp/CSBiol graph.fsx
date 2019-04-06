//create a directed graph from list of verticies and list of edges

//verticies are nodes
//edges are the precedents/dependents

//labelled vertex
type LVertex<'Vertex, 'Label> = 'Vertex * 'Label

type Edge<'Vertex> = 'Vertex * 'Vertex

type Graph<'Vertex,'Label,'Edge> = Map<'Vertex, 

module Verticies = 
    //add and remove
    let add ((v, l): LVertex<'Vertex,'Label>) (g: Graph<'Vertex,'Edge>)
        : Graph<'Vertex,'Label,'Edge> = 
        Map.add v (Map.empty, l, Map.empty) g