open System
open System.Text.RegularExpressions

let check left right = left = right

let parseFloat s =
    try
        float s
    with _ ->
        0.

let tickToDecimalConverter (ticksString: string) =

    let intPart, tick, eighth =
        let split = ticksString.Split '-'
        let intPart = split[0] |> parseFloat
        let tickPart = split[1]
        let tick = tickPart.Substring(0, 2) |> parseFloat

        let eighth =
            tickPart.Substring(2, 1)
            |> function
                | "+" -> 4.
                | other -> other |> parseFloat

        intPart, tick, eighth

    printfn "This translates as: Int: %d Tick %d/32 Eights:%d/256" (int intPart) (int tick) (int eighth)
    intPart + tick / 32. + eighth / 256.

tickToDecimalConverter "102-122" |> check 102.3828125
tickToDecimalConverter "106-00+" |> check 106.015625
tickToDecimalConverter "98-080" |> check 98.25

let tickToDecimalConverter2 ticksString =
    let treasuryStringRegex =
        Regex("^(?<int>\+|-?[0-9]+)-(?<tick>[0-9][0-9]?)(?<eighth>[0-9+]?)$", RegexOptions.Compiled)

    let matched = treasuryStringRegex.Match ticksString

    if matched.Success then
        let intPart = matched.Groups["int"].Value |> parseFloat
        let tick = matched.Groups.["tick"].Value |> parseFloat

        let eighth =
            matched.Groups.["eighth"].Value
            |> function
                | "+" -> 4.
                | other -> parseFloat other

        printfn "This translates as: Int: %d Tick %d/32 Eighths:%d/256" (int intPart) (int tick) (int eighth)
        intPart + tick / 32. + eighth / 256.
    else
        failwithf "Quote not in correct format. Expecting 100-123, e.g. 100 + 12/32 + 3/8 * 1/32"

tickToDecimalConverter2 "102-122" |> check 102.3828125
tickToDecimalConverter2 "106-00+" |> check 106.015625
tickToDecimalConverter2 "98-080" |> check 98.25
