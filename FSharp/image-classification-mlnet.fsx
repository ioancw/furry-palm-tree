#r "nuget:Microsoft.ML"
#r "nuget:Microsoft.ML.Vision"
#r "nuget:Microsoft.ML.ImageAnalytics"
#r "nuget:SciSharp.TensorFlow.Redist"

open System
open System.IO
open Microsoft.ML
open Microsoft.ML.Data
open Microsoft.ML.Vision

//need images from the example.

let prob s i = s * ((365. - (float i)) / 365.)

[ 1 .. 50 ]
|> List.scan prob 1.
|> List.iteri (fun i p -> printfn "Probability of %d people is: %f" i (p * 100.))
