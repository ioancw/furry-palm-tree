type 'a Edge = 'a * 'a

type 'a Graph = 'a list * 'a Edge list

let g = (['b';'c';'d';'f';'g';'h';'k'],[('b','c');('b','f');('c','f');('f','k');('g','h')])

type 'a Node = 'a * 'a list

type 'a AdjacencyGraph = 'a Node list

let ga = [('b',['c'; 'f']); ('c',['b'; 'f']); ('d',[]); ('f',['b'; 'c'; 'k']); 
                                                    ('g',['h']); ('h',['g']); ('k',['f'])]