module IncrementalAndroidExample.UpDownScreen

open FSharp.Data.Adaptive
open Application

let model = cval 0

let update f = transact (fun _ -> model.Value <- f model.Value)

let view ctx dispatch m =
    vbox ctx
        [ button ctx "+" ^ dispatch ^ fun x -> x + 1
          textView ctx (string m)
          button ctx "none" ^ dispatch id
          button ctx "-" ^ dispatch ^ fun x -> x - 1 ]

let view' ctx dispatch =
    AVal.map (view ctx dispatch) model
