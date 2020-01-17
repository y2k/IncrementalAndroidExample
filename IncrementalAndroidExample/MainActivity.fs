namespace IncrementalAndroidExample

open Android.App
open FSharp.Data.Adaptive

[<Activity(Label = "Android Incremental", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity() =
    inherit Activity()
    override this.OnCreate(bundle) =
        base.OnCreate(bundle)

        let mutable reloadUiF = ignore
        let r =
            UpDownScreen.view' this (fun f _ ->
                UpDownScreen.update f
                reloadUiF())

        let rec reloadUi() =
            r
            |> AVal.force
            |> this.SetContentView
        reloadUiF <- reloadUi
        reloadUi()
