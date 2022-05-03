// Write an function findMatches that takes a string and a list of tuples as
// arguments. Each element of the list will be a tuple consisting of a string
// and an int. Find all of the tuples for which the string matches the first
// argument and collect all of the corresponding integers. Your final result
// should be the collected integers sorted in ascending order. For example, if
// the string argument is "A" and the list is [("A",5); ("BB",6); ("AA",9);
// ("A",0)], your function should return the list [0;5].  You may use the
// List.sort function to produce your final result.

type Match = string * int

let findMatches pattern (l: Match list) =
    // Go through every tuple in list and append the associative number if
    // the string is matched with pattern to a new list.
    // Sort the new list to get the correct order
    let values = [for (str, number) in l do if str = pattern then yield number]
    List.sort values

printfn "Hello World from F#!"

findMatches "A" [("A",5); ("BB",6); ("AA",9); ("A",0)] |> Seq.iter (printf "%d ")
printfn ""
