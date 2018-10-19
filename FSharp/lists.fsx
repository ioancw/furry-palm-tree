let rec fibs n =
  match n with
    | 1L -> 1L
    | 2L -> 1L
    | n -> fibs (n - 1L) + fibs (n - 2L)