module IncrementalAndroidExample.UpDownScreen

open FSharp.Data.Adaptive
open Application

module AVal =
    let map4 f a b c d =
        let ab = AVal.map2 (fun a b -> a, b) a b
        let cd = AVal.map2 (fun a b -> a, b) c d
        AVal.map2 (fun (a, b) (c, d) -> f a b c d) ab cd

type Model = Model of int

let view ctx dispatch model =
    AVal.map4
        (fun a b c d -> vbox ctx [ a; b; c; d ])
        (AVal.constant ^ button ctx "+" ^ dispatch ^ fun (Model x) -> Model ^ x + 1)
        (AVal.map (fun (Model m) -> textView ctx (string m)) model)
        (AVal.constant ^ button ctx "none" ^ dispatch id)
        (AVal.constant ^ button ctx "-" ^ dispatch ^ fun (Model x) -> Model (x - 1))
