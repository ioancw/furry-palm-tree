function sort_word(str)
  return join(sort(collect(str)))
end

function get_anagrams(file, hash_function)
  anagrams = Dict()
  open(file,"r") do f
      for line in eachline(f)
          word = chomp(strip(line))
          hashed_key = hash_function(word)

          if haskey(anagrams, hashed_key)
            push!(anagrams[hashed_key],word)
          else
            anagrams[hashed_key] = [word]
          end
      end
  end
  return anagrams
end

fname = "/Users/ioanwilliams/github/furry-palm-tree/words.txt"
anagrams = get_anagrams(fname, sort_word)

sorted = []
for v in values(anagrams)
    push!(sorted, (length(v),v))
end

#sort by first element of tuple
sort!(sorted, by = x -> x[1])

for (a,b) in sorted
  println(a,": ",join(b,","))
end
