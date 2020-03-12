//Programming for the puzzled
//Attempt at doing all of this in F#

let input = ['F';'B';'F';'F';'F';'B';'F';'B';'B';'F';'F';'B';'B';'F' ]

//find the smallest set of sequences to ensure that the input list is all F or B.
//one F is sequence of 1
//F F is sequence of two Fs
//expected output is:
// 1F, 1B, 3F, 1B, 1F, 2B, 2F, 2B, 1F

input
|> 
