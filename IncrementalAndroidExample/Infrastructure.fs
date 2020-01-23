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

    let private createMemoView avalue fnew fpostAction =
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

    let button ctx text onClick =
        let v = new Button(ctx)
        v.Text <- text
        v.Click.Add onClick
        v :> View

    let vbox ctx (vs : #View aval list) =
        let avbox' ctx (children : _ list aval) =
            createMemoView children (fun _ -> new LinearLayout(ctx, Orientation = Orientation.Vertical)) (fun l children ->
                children
                |> List.iteri ^ fun i (ch : #View) ->
                    printfn ""
                    if ch.Parent <> (l :> IViewParent) then
                        if not <| isNull (ch.Parent) then (ch.Parent :?> ViewGroup).RemoveView(ch)
                        if i < l.ChildCount then l.RemoveViewAt i
                        l.AddView(ch, i))

        List.foldBack (fun b a -> AVal.map2 (fun b c -> c :: b) a b) vs (cval [] :> _ list aval) |> avbox' ctx

    let editText ctx onChanged atext =
        let mutable onChangeDisposable =
            { new IDisposable with
                member __.Dispose() = () }
        createMemoView atext (fun _ -> new EditText(ctx)) (fun et text ->
            onChangeDisposable.Dispose()
            et.Text <- text
            onChangeDisposable <- et.AfterTextChanged |> Observable.subscribe ^ fun e -> onChanged (string e.Editable))

    let textView ctx atext =
        createMemoView atext (fun _ -> new TextView(ctx)) (fun v text -> v.Text <- text)
