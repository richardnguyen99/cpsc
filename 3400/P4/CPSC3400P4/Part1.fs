// Write a function maxCubeVolume that takes a list of floating-point tuples
// that represent dimensions of a cube (length, width, and height) and returns
// the volume of the cube that has the largest volume.  Each tuple consists of
// three floating point values that are all greater than zero.  The volume of
// the cube is computed using (length*width*height). If the list is empty, return 0.0
//
// author: Minh-Hieu Nguyen

open System

let maxCubeVolume cubes : float =
    // function to compute volume
    let volume cube : float =
        // Extract data from the passed-in cube
        let (length, width, height) = cube
        (length * width * height)

    // Compute volumes and store them
    let _volumes = [for cube in cubes do yield (volume cube)]

    // Helper to compute the max float in volume list
    let maxFloat list =
        let rec loop max = function
            | [] -> max
            | head :: tail -> loop (if head > max then head else max) tail
        loop 0.0 list

    maxFloat _volumes

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    printfn "%f." (maxCubeVolume [(2.1, 3.4, 1.8); (4.7, 2.8, 3.2); (0.9, 6.1, 1.0); (3.2, 5.4, 9.9)])

    0 // return an integer exit code

        
