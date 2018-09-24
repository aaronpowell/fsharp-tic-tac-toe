module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch

open Shared

open Fulma

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
| PlayMove of Position
| InitialGameLoaded of Result<Board, exn>


// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
    let initialModel = { Rows = [1..3]; Cols = ['A'..'C']; Board = Map.empty }
    let loadCountCmd =
        Cmd.ofPromise
            (fetchAs<Board> "/api/init")
            []
            (Ok >> InitialGameLoaded)
            (Error >> InitialGameLoaded)
    initialModel, loadCountCmd

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | PlayMove position ->
        let nextModel = { currentModel with Board = Map.add position 'X' currentModel.Board }
        nextModel, Cmd.none
    | InitialGameLoaded (Ok initialBoard)->
        let nextModel = { Rows = [1..3]; Cols = ['A'..'C']; Board = initialBoard }
        nextModel, Cmd.none

    | _ -> currentModel, Cmd.none


let safeComponents =
    let components =
        span [ ]
           [
             a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io/elmish/" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://mangelmaxime.github.io/Fulma" ] [ str "Fulma" ]
           ]

    p [ ]
        [ strong [] [ str "SAFE Template" ]
          str " powered by: "
          components ]

let view (model : Model) (dispatch : Msg -> unit) =
    div []
        [ Navbar.navbar [ Navbar.Color IsPrimary ]
            [ Navbar.Item.div [ ]
                [ Heading.h2 [ ]
                    [ str "Tic Tac Toe" ] ] ]

          Container.container [] [
                  Table.table [ Table.IsFullWidth ] [
                    thead [] [
                        tr [] [
                            th[] []
                            th[] [ str "A" ]
                            th[] [ str "B" ]
                            th[] [ str "C" ]
                        ]
                     ]
                    tbody [] [
                        tr [] [
                            td [] [ str "1" ]
                            td [] [ str "" ]
                            td [] [ str "" ]
                            td [] [ str "" ]
                        ]
                        tr [] [
                            td [] [ str "2" ]
                            td [] [ str "" ]
                            td [] [ str "" ]
                            td [] [ str "" ]
                        ]
                        tr [] [
                            td [] [ str "2" ]
                            td [] [ str "" ]
                            td [] [ str "" ]
                            td [] [ str "" ]
                        ]
                    ]
                  ]
              ]

          Footer.footer [ ]
                [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]


#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
