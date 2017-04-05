function sort_word(str)
  return join(sort(collect(str)))
end

fname = "/Users/ReallyBigIoan/Documents/words.txt"
# using do means the file is closed automatically
# in the same way "with" does in python
anagrams = Dict()
open(fname,"r") do f
    for line in eachline(f)
        word = chomp(strip(line))
        sorted_word = sort_word(word)

        if haskey(anagrams, sorted_word)
          push!(anagrams[sorted_word],word)
        else
          anagrams[sorted_word] = [word]
        end
    end
end

sorted = []
for v in values(anagrams)
    push!(sorted, (length(v),v))
end

#sort by first element of tuple
sort!(sorted, by = x -> x[1])

for(a,b) in sorted
  println(a,": ",join(b,","))
end
