open System

let upper (s: string) = s.ToUpper()

let keyschedule key =
    let s = key |> upper |> Seq.toArray 
            |> Array.filter Char.IsLetter
    let l = Array.length s
    (fun n -> int s.[n % l])

let enc k c = ((c + k - 130) % 26) + 65
let dec k c = ((c - k + 130) % 26) + 65
let crypt f key = Array.mapi (fun n c -> f (key n) c |> char)

let encrypt key plaintext =
    plaintext |> upper |> Seq.toArray
    |> Array.filter Char.IsLetter
    |> Array.map int
    |> crypt enc (keyschedule key)
    |> String

let decrypt key ciphertext =
    ciphertext |> upper |> Seq.toArray
    |> Array.map int
    |> crypt dec (keyschedule key)
    |> String
 
let key = "Vigenere Cipher"

let keyEncrypt = keyschedule key
let plaintext = "Beware the Jabberwock, my son! The jaws that bite, the claws that catch!"

plaintext
|> upper |> Seq.toArray //conver to upper case and to an array of characters
|> Array.filter Char.IsLetter //remove non characters
|> Array.map int //convert char to int
|> crypt enc (keyschedule key)



let cipher = encrypt key plaintext
let plain = decrypt key cipher
printfn "%s\n%s" cipher plain