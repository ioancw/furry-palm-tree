s0 = 100.0
r = 0.05
T = 1.0
σ = 0.2

values = Float64[]

for simulation in 1:10^6
    st = s0 * exp((r - 0.5 * σ^2) * T + σ * 1 * sqrt(T))
    #push!(values, st)
end

println(length(values))
