module Views

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Actions
open Shared

open Fulma

let private safeComponents =
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

let private renderView dispatch pos cell serverMoving =
    let cellToStr cell =
        match cell with
        | X -> "X"
        | O -> "O"
        | Blank -> ""


    if serverMoving then
        td [] [ cellToStr cell |> str ]
    else
        td [ OnClick(fun _ -> PlayMove(pos) |> dispatch)] [ cellToStr cell |> str ]

let private renderCell dispatch pos state =
    match Map.tryFind pos state.Board with
    | Some cell ->
        renderView dispatch pos cell state.ServerMoving
    | _ ->
        renderView dispatch pos Blank state.ServerMoving

let view model dispatch =
    div []
        [ Navbar.navbar [ Navbar.Color IsPrimary ]
            [ Navbar.Item.div [ ]
                [ Heading.h2 [ ]
                    [ str "Tic Tac Toe" ] ] ]

          Container.container [] [
                  Table.table [ Table.IsFullWidth ] [
                    thead [] [
                        tr [] [
                            yield th[] []
                            for col in model.Cols ->
                                th[] [ string col |> str ]
                        ]
                     ]
                    tbody [] [
                        for row in model.Rows ->
                            tr [] [
                              yield th [] [ string row |> str ]
                              for col in model.Cols -> renderCell dispatch (col, row) model
                            ]
                    ]
                  ]
              ]

          Footer.footer [ ]
                [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]

