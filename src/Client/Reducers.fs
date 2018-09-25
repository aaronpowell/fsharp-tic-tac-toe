module Reducers

open Actions
open Shared

open Elmish

open Fable.PowerPack.Fetch
open Fable.Core.JsInterop

let init () =
    let initialModel = { Rows = [1..3]; Cols = ['A'..'C']; Board = Map.empty; ServerMoving = true }
    let loadCountCmd =
        Cmd.ofPromise
            (fetchAs<Board> "/api/init")
            []
            (Ok >> InitialGameLoaded)
            (Error >> InitialGameLoaded)
    initialModel, loadCountCmd

let private playClientMove currentModel pos =
    let nextModel = { currentModel with Board = Map.add pos X currentModel.Board; ServerMoving = true }

    let defaultProps =
        [ RequestProperties.Method HttpMethod.POST
        ; requestHeaders [ContentType "application/json"]
        ; RequestProperties.Body <| unbox(toJson nextModel)]

    let playServer =
        Cmd.ofPromise
            (fetchAs<Position> "/api/move")
            defaultProps
            (Ok >> ServerMove)
            (Error >> ServerMove)

    nextModel, playServer

let update msg currentModel =
    match msg with
    | PlayMove position ->
        match currentModel.Board |> Map.tryFind position with
        | Some _ -> currentModel, Cmd.none
        | None -> playClientMove currentModel position
    | InitialGameLoaded (Ok initialBoard)->
        let nextModel = { Rows = [1..3]; Cols = ['A'..'C']; Board = initialBoard; ServerMoving = false }
        nextModel, Cmd.none
    | ServerMove (Ok pos) ->
        let nextModel = { currentModel with Board = Map.add pos O currentModel.Board; ServerMoving = false }
        nextModel, Cmd.none

    | _ -> currentModel, Cmd.none
