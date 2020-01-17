namespace IncrementalAndroidExample

open Android.App
open FSharp.Data.Adaptive

[<Activity(Label = "Android Incremental", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity() =
    inherit Activity()
    override this.OnCreate(bundle) =
        base.OnCreate(bundle)
        let rec reloadUi() =
            UpDownScreen.view' this (fun f _ ->
                UpDownScreen.update f
                reloadUi())
            |> AVal.force
            |> this.SetContentView
        reloadUi()
