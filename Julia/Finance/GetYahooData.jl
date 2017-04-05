using Base.Dates
using Requests
using DataFrames
using TimeSeries

abstract DataSource
immutable YahooDataSource <: DataSource end

function get_historical_data(::YahooDataSource, startDate::Date, endDate::Date, symbol::AbstractString)
  startDate < endDate || error("dates are wrong")

  url = "http://chart.finance.yahoo.com/table.csv?s="

  query = "$symbol&a=$(Int(Month(startDate)) - 1)&b=$(Int(Day(startDate)))&c=$(Int(Year(startDate)))&d=$(Int(Month(endDate)) - 1)&e=$(Int(Day(endDate)))&f=$(Int(Year(endDate)))&g=d"
  urlFull = string(url, query, "&ignore=.csv")
  println(urlFull)
  r = get(urlFull)
  df = readtable(IOBuffer(readstring(r)))
  df[:Date] = Date(df[:Date])
  return df
end

function get_data_yahoo(symbol::AbstractString, startDate::Date, endDate::Date)
  return get_historical_data(YahooDataSource(), startDate, endDate, symbol)
end

function read_dax_data()
  DAX = get_historical_data(YahooDataSource(), Date(2016, 3, 24), Date(2017, 2, 28), "AAPL")

  # convert to time series for calc
  #ts = TimeArray(DAX[:Date].data, DAX[:Adj_Close], ["returns"])
  #tsCalc = log(ts["returns"] ./ lag(ts["returns"], padding=true))
  #DAX[:returns] = tsCalc.values
  #DAX[isnan(DAX[:returns]), :returns] = 0.0

  # Realized volatility (e.g. as defined for variance swaps)
  #DAX[:rea_var] = 252 * cumsum(DAX[:returns] .^ 2) ./ range(1, size(DAX)[1])
  #DAX[:rea_vol] = sqrt(DAX[:rea_var])
  println(DAX)
  return DAX
end

println(get_data_yahoo("AAPL",Date(2016, 3, 24), Date(2017, 2, 28)))
