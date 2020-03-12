//chapter 2

let splitAtSpaces (text: string) = 
    text.Split ' '
    |> Array.toList

let wordCount text = 
    let words = splitAtSpaces text
    let numWords = words.Length
    let distinctWords = List.distinct words
    let numDups = numWords - distinctWords.Length
    (numWords, numDups)

let showWordsCount text = 
    let numWords, numDups = wordCount text
    printfn "--> %d words in text" numWords
    printfn "--> %d duplicate words" numDups

let (numWords, numDups) = wordCount "All the king's horses and all the king's men"

showWordsCount "Couldn't put Humpty together again"

splitAtSpaces "hello world"

let showResults (numWords, numDups) =
    printfn "--> %d words in the text" numWords
    printfn "--> %d duplicate words" numDups

let showWordCount2 text = showResults (wordCount text)

showWordCount2 "ioan ceredig williams"