module IncrementalAndroidExample.UpDownScreen

open FSharp.Data.Adaptive
open Infrastructure
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
        vbox ctx
            [ AVal.constant ^ button ctx "+" ^ fun _ -> dispatch (operation ^ (+) 1)
              model
              |> AVal.map fget
              |> AVal.map string
              |> textView ctx
              AVal.constant ^ button ctx "none" ^ fun _ -> dispatch id
              AVal.constant ^ button ctx "+" ^ fun _ -> dispatch (operation ^ flip (-) 1) ]

    let view ctx model dispatch =
        vbox ctx
            [ viewCounter ctx model dispatch (fun x -> x.counter1) (fun x y -> { x with counter1 = y })
              textView ctx ^ AVal.constant "\n===================================\n"
              viewCounter ctx model dispatch (fun x -> x.counter2) (fun x y -> { x with counter2 = y }) ]
