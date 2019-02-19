open System
open System.Text.RegularExpressions
open System

let testAssert a b = a = b

let lastDayOfMonth (d: DateTime) = 
    DateTime(d.Year, d.Month, DateTime.DaysInMonth(d.Year, d.Month))

let futuresMonthIndex month =
    ["F";"G";"H";"J";"K";"M";"N";"Q";"U";"V";"X";"Z"] 
    |> List.findIndex (fun m -> m = month ) 
    |> (+) 1

let getFuturesYear (code: string) (today: DateTime) = 
    let futuresMatched = Regex("^(?<month>[FGHJKMNQUVXZ])(?<year>[0-9][0-9]?)$").Match(code)

    if futuresMatched.Success then
        let year = futuresMatched.Groups.["year"].Value 
        let yearsMod = 10.0 ** float(String.length year) |> int

        let expiryDate = 
            let yearPart = year |> int
            let monthPart = futuresMatched.Groups.Item("month").Value |> futuresMonthIndex
            let newYear = today.Year - today.Year % yearsMod + yearPart
            DateTime(newYear, monthPart, 1)

        if lastDayOfMonth(expiryDate) < today then
            Some (DateTime(expiryDate.Year + yearsMod, expiryDate.Month, expiryDate.Day))
        else 
            Some expiryDate
    else
        None

let getTwoDigitCode expiryCode twoDigitYearEnabled today= 
    let futuresExpiryYear = getFuturesYear expiryCode today 
    match futuresExpiryYear with
    | Some d -> if d.Year < twoDigitYearEnabled then
                    expiryCode
                else   
                    let year = (d.Year % 100).ToString()
                    let x = expiryCode.[0].ToString()
                    x + year
    | None -> "No date"

let getValidYear = function
    | Some (d: DateTime) -> d.Year
    | None _-> 1969

DateTime(2018, 4, 28) |> getFuturesYear "H28" |> getValidYear |> testAssert 2028
DateTime(2018, 3, 28) |> getFuturesYear "H8" |> getValidYear |> testAssert 2018
DateTime(2018, 4, 28) |> getFuturesYear "H0" |> getValidYear |> testAssert 2020
DateTime(2018, 2, 28) |> getFuturesYear "H8" |> getValidYear |> testAssert 2018
DateTime(2018, 3, 2) |> getFuturesYear "H8" |> getValidYear |> testAssert 2018
DateTime(2018, 3, 26) |> getFuturesYear "H8" |> getValidYear |> testAssert 2018
DateTime(2018, 3, 2) |> lastDayOfMonth |> getFuturesYear "H8" |> getValidYear |> testAssert 2018
DateTime(2018, 3, 2) |> lastDayOfMonth |> getFuturesYear "H28" |> getValidYear |> testAssert 2028
DateTime(2018, 4, 1) |> getFuturesYear "H8" |> getValidYear |> testAssert 2028


DateTime(2018, 3, 26) |> getTwoDigitCode "H8" 2024 |> testAssert "H8"
DateTime(2018, 4, 26) |> getTwoDigitCode "H8" 2024 |> testAssert "H8"
DateTime(2018, 1, 28) |> getTwoDigitCode "H4" 2024 |> testAssert "H24"
DateTime(2014, 3, 36) |> getTwoDigitCode "H4" 2024 |> testAssert "H4"
DateTime(2014, 4, 1) |> getTwoDigitCode "H4" 2024 |> testAssert "H24"
DateTime(2024, 4, 1) |> getTwoDigitCode "H4" 2024 |> testAssert "H34"
