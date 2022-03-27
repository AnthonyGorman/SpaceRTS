module GameLogic.Apply

module Option = 
    let apply fOpt xOpt =
        match fOpt, xOpt with
        | Some f, Some x -> Some (f x)
        | _ -> None

let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)
