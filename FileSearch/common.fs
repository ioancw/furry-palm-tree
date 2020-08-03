module common

open System

//Index Module
type Token = Token of string

type Path = Path of string

type Ngram = Ngram of string

type Document = { Path: Path; Tokens: Token [] }

type Index =
    { Ngrams: Map<Ngram, Token []>
      Tokens: Map<Token, Path []> }

type Query = { QueryText: string; Index: Index }

let stringParse delimiter (s: string) = s.Split([| delimiter |])

let parseTilde = stringParse '~'

let parseColon = stringParse ':'

let parseComma = stringParse ','

let stringJoin (delimiter: char) (items: string []) = String.Join(delimiter, items)

let arrayJoinComma = stringJoin ','

let arraySecond (a: 'a []) = a.[1]

module Colours =
    let magenta = ConsoleColor.Magenta
    let green = ConsoleColor.Green
    let red = ConsoleColor.Red
    let yellow = ConsoleColor.Yellow
    let gray = ConsoleColor.Gray
    let darkgreen = ConsoleColor.DarkGreen
    let darkyellow = ConsoleColor.DarkYellow

let apply colour = Console.ForegroundColor <- colour

let printc colour text = 
    let current = Console.ForegroundColor
    apply colour
    printf "%s" text
    apply current