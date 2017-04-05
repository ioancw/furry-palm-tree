module SackPacking

  type Sack
      sum::Int64
      boxes::Vector{Int64}
  end

  function add_box(sack::Sack,item::Int64)
      sack.sum = sack.sum + item
      push!(sack.boxes, item)
  end

  function pack_sleigh(boxes::Vector{Int64}, volume::Int64)
    sleigh = Sack[]
    sort!(boxes, rev=true)
    for box in boxes
      newSack = true
      for sack in sleigh
        if sack.sum + box <= volume
          add_box(sack, box)
          newSack = false
          break
        end
      end

      if newSack
        sack = Sack(0,Int64[])
        add_box(sack, box)
        push!(sleigh, sack)
      end

    end
    return length(sleigh)
  end

  sim1 = Int64[]
  simulation = 0
  volume = 1000000

  for simulation in 1:10^6
    randomBoxes = rand(1:volume,90)
    push!(sim1, pack_sleigh(randomBoxes, volume))
  end

  println("Simulation: ", simulation)
  println("Mean Sim1: ", mean(sim1))
end
