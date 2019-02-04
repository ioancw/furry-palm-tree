
#I @"/Users/ioanwilliams/Projects/Matricies/packages/MathNet.Numerics.4.7.0/lib/net40/"
#r @"MathNet.Numerics.dll"
#I @"/Users/ioanwilliams/Projects/Matricies/packages/MathNet.Numerics.FSharp.4.7.0/lib/net45/"
#r "MathNet.Numerics.FSharp.dll"

open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra

fsi.AddPrinter(fun (matrix:Matrix<float>) -> matrix.ToString())
fsi.AddPrinter(fun (matrix:Matrix<float32>) -> matrix.ToString())
fsi.AddPrinter(fun (matrix:Matrix<complex>) -> matrix.ToString())
fsi.AddPrinter(fun (matrix:Matrix<complex32>) -> matrix.ToString())
fsi.AddPrinter(fun (vector:Vector<float>) -> vector.ToString())
fsi.AddPrinter(fun (vector:Vector<float32>) -> vector.ToString())
fsi.AddPrinter(fun (vector:Vector<complex>) -> vector.ToString())
fsi.AddPrinter(fun (vector:Vector<complex32>) -> vector.ToString())

let m : Matrix<float> = DenseMatrix.randomStandard 50 50
(m * m.Transpose()).Determinant()

let m1 = DenseMatrix.init 4 4 (fun i j -> float i * 100.0 + float j)

let getSVD (m: Matrix<float>) = 
    let svd = m.Svd(true)
    let U, sigmas, Vt = svd.U, svd.S, svd.VT
    U, sigmas, Vt

let U, sigmas, Vt = getSVD m