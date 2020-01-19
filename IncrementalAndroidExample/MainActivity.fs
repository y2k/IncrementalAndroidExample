namespace IncrementalAndroidExample

open Android.App
open FSharp.Data.Adaptive

module Screen = TodoListScreen

[<Activity(Label = "Android Incremental", MainLauncher = true, Icon = "@mipmap/icon")>]
type MainActivity() =
    inherit Activity()
    override this.OnCreate(bundle) =
        base.OnCreate(bundle)

        let model = cval <| Screen.ViewModel.init

        let mutable reloadUiF = ignore

        let avalView =
            Screen.View.view this model (fun f ->
                transact (fun _ -> model.Value <- f model.Value)
                reloadUiF())
            
        let mutable prevContentView : Android.Views.View option = None

        let rec reloadUi() =
            avalView
            |> AVal.force
            |> fun view ->
                if Some view <> prevContentView then
                    this.SetContentView view
                    prevContentView <- Some view

        reloadUiF <- reloadUi
        reloadUi()
