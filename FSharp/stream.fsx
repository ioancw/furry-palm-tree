"williams"
|> Seq.windowed 4
|> Seq.map (fun chars ->
    chars
    |> Array.take 4
    |> Array.map string
    |> Array.reduce (+))
|> Seq.toArray


[|'w'; 'i'; 'l'; 'l'|]
|> Array.map string
|> Array.reduce (+)

let stringConcat (chars: char[]) = 
    System.String.Concat(chars)