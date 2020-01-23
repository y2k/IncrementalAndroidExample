module IncrementalAndroidExample.TodoListScreen

open System
open FSharp.Data.Adaptive
open Infrastructure
open Infrastructure.Dsl

type Todo = Todo of string

type ViewModel =
    { text : string
      todos : Todo IndexList }
    static member init =
        { text = ""
          todos = IndexList.empty }

module Domain =
    let edit text (vm : ViewModel) =
        { vm with text = text }

    let clear (vm : ViewModel) =
        { vm with todos = IndexList.empty }

    let testRandom (vm : ViewModel) =
        { vm with text = sprintf "%O" ^ Random().Next() }

    let addTodo (vm : ViewModel) =
        { vm with
              text = ""
              todos = vm.todos.Add ^ Todo vm.text }

module View =
    let viewItem ctx (x : Todo aval) =
        x
        |> AVal.map ^ fun (Todo x) -> sprintf "'%s'" x
        |> textView ctx

    let viewItems ctx (todos : Todo alist) =
        todos
        |> AList.map ^ AVal.constant
        |> AList.map ^ viewItem ctx
        |> AList.toAVal
        |> AVal.bind (Seq.toList >> vbox ctx)

    let view ctx (amodel : ViewModel aval) dispatch =
        [ amodel
          |> AVal.map ^ fun m -> m.text
          |> editText ctx ^ fun e -> dispatch ^ Domain.edit e
          AVal.constant ^ button ctx "Random" ^ fun _ -> dispatch Domain.testRandom
          AVal.constant ^ button ctx "Add" ^ fun _ -> dispatch Domain.addTodo
          AVal.constant ^ button ctx "Remove all" ^ fun _ -> dispatch Domain.clear
          amodel
          |> AVal.map ^ fun m -> m.todos
          |> AList.ofAVal
          |> viewItems ctx ]
        |> vbox ctx
