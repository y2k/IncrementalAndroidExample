module IncrementalAndroidExample.UpDownScreen

open FSharp.Data.Adaptive
open Infrastructure
open Infrastructure.Utils
open Infrastructure.Dsl

type Model = Model of int

let view ctx dispatch model =
    avbox ctx
        [ AVal.constant ^ button ctx "+" ^ dispatch ^ fun (Model x) -> Model(x + 1)
          AVal.map (fun (Model x) -> textView ctx (string x)) model
          AVal.constant ^ button ctx "none" ^ dispatch id
          AVal.constant ^ button ctx "-" ^ dispatch ^ fun (Model x) -> Model(x - 1) ]
