namespace IncrementalAndroidExample

open Android.App
open FSharp.Data.Adaptive

[<Activity(Label = "Android Incremental", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity() =
    inherit Activity()
    override this.OnCreate(bundle) =
        base.OnCreate(bundle)
        
        let model = cval <| UpDownScreen.Model 0

        let mutable reloadUiF = ignore
        let avalView =
            UpDownScreen.view this (fun f _ ->
                transact (fun _ -> model.Value <- f model.Value) 
                reloadUiF())
                model

        let rec reloadUi() =
            avalView
            |> AVal.force
            |> this.SetContentView
        reloadUiF <- reloadUi
        reloadUi()
