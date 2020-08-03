(*** raw ***)
---
layout: post
title:  "Finding anagrams with F#"
description: "Finding all anagrams of a word in a dictionary."
date:   2020-06-14 06:07:00 +0100
categories: 
---

(**
# Anagrams
Recently someone asked me how to find all anagrams of a given word contained in a dictionary,
i.e. a file containing a large numnber of words, one per line.

While I have an implementation in Python, I thought I'd try it in F#.

The basic way to solve this problem, is to find a means to determine a key/hash for each 
word and then storing it in a dictionary or map, along with the anagrams that match the 
given key.

One can use a fancy method to determine the key, e.g. create a prime number hash, based on 
position of each letter in the alphabet, but for now, it's as easy to sort each word, as all 
anagrams of a given word, sort to the same string.

This can be done easily in F#, as all the magic happens inside Array.groupBy.  
This takes the array of words read from the file and creates a dictionary where the key is 
the sorted word (all anagrams sort to the same string) and the value is an array of all 
words with the same key (i.e. anagrams). They dictionary is then converted to an Sequence.

I don't believe F# has it's own sort string function, so you have to write your own.

*)

open System.IO

let rec listToString l =
    match l with
    | [] -> ""
    | head :: tail -> head.ToString() + listToString tail

let sortString s =
    s |> Seq.sort |> Seq.toList |> listToString

let fileName =
    @"/Users/ioanwilliams/github/furry-palm-tree/words.txt"

fileName
|> File.ReadAllLines
|> Seq.groupBy sortString
|> Seq.sortByDescending (snd >> Seq.length)
|> Seq.take 10
|> Seq.iter (fun (k, vs) -> vs |> String.concat "," |> printfn "%s : %s" k)
