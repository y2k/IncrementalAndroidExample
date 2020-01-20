module IncrementalAndroidExample.UpDownScreen

open FSharp.Data.Adaptive
open Infrastructure
open Infrastructure.Utils
open Infrastructure.Dsl

type ViewModel =
    { counter1 : int
      counter2 : int }
    static member init =
        { counter1 = 0
          counter2 = 0 }

module View =
    let viewCounter ctx model dispatch fget fset =
        let operation op x =
            x
            |> (fget
                >> op
                >> fset x)
        avbox ctx
            [ AVal.constant ^ button ctx "+" ^ fun _ -> dispatch (operation ^ (+) 1)
              model
              |> AVal.map fget
              |> AVal.map ^ fun x -> textView ctx (string x)
              AVal.constant ^ button ctx "none" ^ fun _ -> dispatch id
              AVal.constant ^ button ctx "+" ^ fun _ -> dispatch (operation ^ flip (-) 1) ]

    let view ctx model dispatch =
        avbox ctx
            [ viewCounter ctx model dispatch (fun x -> x.counter1) (fun x y -> { x with counter1 = y })
              AVal.constant ^ textView ctx "\n===================================\n"
              viewCounter ctx model dispatch (fun x -> x.counter2) (fun x y -> { x with counter2 = y }) ]
