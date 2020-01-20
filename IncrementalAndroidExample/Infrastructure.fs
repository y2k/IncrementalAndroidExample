module IncrementalAndroidExample.Infrastructure

open System
open FSharp.Data.Adaptive

[<AutoOpen>]
module Prelude =
    [<Obsolete>]
    let inline TODO() = failwith "TODO"

    let inline (^) f x = f x
    let inline flip f a b = f b a

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

    let editText' ctx text onChanged =
        let v = new EditText(ctx)
        v.Text <- text
        let d =
            v.AfterTextChanged |> Observable.subscribe ^ fun e -> onChanged (e.Editable.ToString())
        v :> View, d

    let textView ctx text =
        let v = new TextView(ctx)
        v.Text <- text
        v :> View

module Utils =
    open Android.Views
    open Android.Widget

    let dispatch (vm : _ cval) f e = transact ^ fun _ -> vm.Value <- f vm.Value e
    let dispatch0 (vm : _ cval) f _ = transact ^ fun _ -> vm.Value <- f vm.Value

    module AVal =
        let map4 f a b c d =
            let ab = AVal.map2 (fun a b -> a, b) a b
            let cd = AVal.map2 (fun a b -> a, b) c d
            AVal.map2 (fun (a, b) (c, d) -> f a b c d) ab cd

    let createLazyView avalue fnew fpostAction =
        let mutable cached = None
        avalue
        |> AVal.map ^ fun value ->
            let l =
                cached
                |> Option.defaultWith ^ fun _ ->
                    let l = fnew()
                    cached <- Some l
                    l
            fpostAction l value
            l :> View

    let avbox ctx (vs : #View aval list) =
        let avbox' ctx (children : _ list aval) =
            createLazyView children (fun _ -> new LinearLayout(ctx, Orientation = Orientation.Vertical)) (fun l children ->
                children
                |> List.iteri ^ fun i (ch : #View) ->
                    printfn ""
                    if ch.Parent <> (l :> IViewParent) then
                        if not <| isNull (ch.Parent) then (ch.Parent :?> ViewGroup).RemoveView(ch)
                        if i < l.ChildCount then l.RemoveViewAt i
                        l.AddView(ch, i))

        List.foldBack (fun b a -> AVal.map2 (fun b c -> c :: b) a b) vs (cval [] :> _ list aval) |> avbox' ctx

    let editText ctx atext onChanged =
        let mutable cached : EditText option = None

        let mutable onChangeDisposable =
            { new IDisposable with
                member __.Dispose() = () }

        AVal.map (fun text ->
            printfn ""
            cached
            |> Option.map ^ fun et ->
                printfn ""
                onChangeDisposable.Dispose()
                et.Text <- text
                onChangeDisposable <- et.AfterTextChanged |> Observable.subscribe ^ fun e -> onChanged (string e.Editable)
                et
            |> Option.defaultWith ^ fun _ ->
                printfn ""
                let et = new EditText(ctx)
                et.Text <- text
                onChangeDisposable <- et.AfterTextChanged |> Observable.subscribe ^ fun e -> onChanged (string e.Editable)
                cached <- Some et
                et
            |> fun x -> x :> View) atext
