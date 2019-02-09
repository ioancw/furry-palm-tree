function getInverseFromSVD(J, tol)
    u, σ, v = svd(J)
    rank = sum(σ.> tol * maximum(σ))
    σ_rank_inv = [ 1/x for x in σ[1:rank]]
    Σ_inv = Diagonal(σ_rank_inv)
    J_inv = v[:,1:rank] * Σ_inv * u'[1:rank,:]
end

function invertEigenValues(λ, tol)
    λ_inv = [(abs(x/λ[end]))^0.5 >= tol ? 1/x : 0 for x in λ]
    Diagonal(λ_inv)
end

function invertEigenValues2(λ, tol)
    λ_inv = [x >= tol ? 1/x : 0 for x in λ]
    Diagonal(λ_inv)
end;

function getEigenDecompInverse(J, tol)
    λ, V = eig(J'*J)
    λ_inv = invertEigenValues(λ, tol)
    J_inv = V * λ_inv * V' * J'
end

#J = readcsv("/Users/ioanwilliams/Downloads/USDSmallMatrix.csv")
J = readcsv("/Users/ioanwilliams/Downloads/matrix-large.csv")

J_inv = pinv(J, 1e-4)
J_inv_e = getEigenDecompInverse(J, 1e-4)
J_inv_svd = getInverseFromSVD(J, 1e-4)

TwentyYr = J_inv[:,28]
