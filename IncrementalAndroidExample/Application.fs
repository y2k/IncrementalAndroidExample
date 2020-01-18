module IncrementalAndroidExample.Application

[<AutoOpen>]
module Prelude =
    let inline (^) f x = f x
    let inline flip f a b = f b a

[<AutoOpen>]
module Dsl =
    open Android.Widget
    open Android.Views

    let vbox ctx children =
        let l = new LinearLayout(ctx)
        l.Orientation <- Orientation.Vertical
        children
        |> List.iter ^ fun (ch : #View) ->
            if not <| isNull (ch.Parent) then (ch.Parent :?> ViewGroup).RemoveView(ch)
            l.AddView(ch)
        l :> View

    let button ctx text onClick =
        let v = new Button(ctx)
        v.Text <- text
        v.Click.Add onClick
        v :> View

    let editText ctx text onChanged =
        let v = new EditText(ctx)
        v.Text <- text
        v.AfterTextChanged.Add ^ fun e -> onChanged (e.Editable.ToString())
        v :> View

    let textView ctx text =
        let v = new TextView(ctx)
        v.Text <- text
        v :> View

open FSharp.Data.Adaptive

type Todo = Todo of string

type Database =
    { todos : Todo list }

let database = cval { todos = [] }

module Domain =
    let add db item =
        { db with todos = (Todo item) :: db.todos }

type ViewModel =
    { text : string
      todos : Todo list }

module View =
    let vm =
        cval
            { text = ""
              todos = [] }

    let view ctx model =
        vbox ctx
            [ editText ctx model.text ^ fun t -> transact ^ fun _ -> vm.Value <- { vm.Value with text = t }
              button ctx "Add" ^ fun _ ->
                  transact ^ fun _ ->
                      vm.Value <-
                          { vm.Value with
                                text = ""
                                todos = (Todo vm.Value.text) :: vm.Value.todos }
              button ctx "Clear" ^ fun _ -> transact ^ fun _ -> vm.Value <- { vm.Value with todos = [] }
              vbox ctx [ yield! model.todos |> List.map ^ fun (Todo x) -> textView ctx x ] ]
