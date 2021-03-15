#r "nuget: Deedle"
open Deedle

let prices =  Frame.ReadCsv(@"C:\Users\ioan_\GitHub\PYBOR\price_ladder.csv")
prices.Columns
prices.ColumnKeys //gets column names
prices.GetColumn<string>("Instrument").

type InstrumentValue = {Instrument: string; Value: float}
let frame = 
    [
        {Instrument="USD_LIBOR_3M:10Y"; Value=0.05}
        {Instrument="USD_LIBOR_3M:10Y"; Value=0.05}
        {Instrument="USD_LIBOR_3M:10Y"; Value=0.05}
        {Instrument="USD_LIBOR_3M:11Y"; Value=0.06}
    ]
    |> Frame.ofRecords

let vals = frame?Value //to get at numerical values
let insts = frame.GetColumn<string>("Instrument") //to get other columns

