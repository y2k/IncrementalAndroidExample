module IncrementalAndroidExample.TodoListScreen

open System
open FSharp.Data.Adaptive
open Infrastructure
open Infrastructure.Utils
open Infrastructure.Dsl

type Todo = Todo of string

type ViewModel =
    { text : string cval
      todos : Todo clist }
    static member init =
        { text = cval ""
          todos = clist [] }

module Domain =
    let edit e (vm : ViewModel) =
        vm.text.Value <- e
        vm

    let clear (vm : ViewModel) =
        vm.todos.Clear()
        vm

    let testRandom (vm : ViewModel) =
        vm.text.Value <- sprintf "%O" ^ Random().Next()
        vm

    let addTodo (vm : ViewModel) =
        vm.todos.Add ^ Todo vm.text.Value |> ignore
        vm.text.Value <- ""
        vm

module View =
    let viewItem ctx (Todo x) =
        atextView ctx ^ AVal.constant ^ sprintf "'%s'" x

    let viewItems ctx (todos : Todo alist) =
        todos
        |> AList.map ^ viewItem ctx
        |> AList.toAVal
        |> AVal.bind (Seq.toList >> avbox ctx)

    let view ctx amodel dispatch =
        [ AVal.bind (fun model -> editText ctx model.text (fun e -> dispatch ^ Domain.edit e)) amodel
          AVal.constant ^ button ctx "Random" ^ fun _ -> dispatch Domain.testRandom
          AVal.constant ^ button ctx "Add" ^ fun _ -> dispatch Domain.addTodo
          AVal.constant ^ button ctx "Remove all" ^ fun _ -> dispatch Domain.clear
          AVal.bind (fun model -> viewItems ctx model.todos) amodel ]
        |> avbox ctx
