J_small = readcsv("/Users/ioanwilliams/Library/Mobile Documents/com~apple~CloudDocs/small_matrix_eur.csv")

J_small_clean = zeros(J_small, Float64)

m, n = size(J_small)
for i = 1:m, j = 1:n
    if J_small[i,j] > 0.01
        J_small_clean[i,j] = J_small[i,j]
    end
end

J_T_x_J = J_small_clean' * J_small_clean
λ, V = eig(J_T_x_J)
λ_Diag = Diagonal(λ)
