module Views

open Fable.Helpers.React
open Fable.Helpers.React.Props

open Actions
open Models

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

let private playerToStr cell =
    match cell with
    | X -> "X"
    | O -> "O"
    | Blank -> ""

let private renderView dispatch pos cell serverMoving =
    if serverMoving then
        td [] [ playerToStr cell |> str ]
    else
        td [ OnClick(fun _ -> PlayMove(pos) |> dispatch)] [ playerToStr cell |> str ]

let private renderCell dispatch pos state =
    match Map.tryFind pos state.Board with
    | Some cell ->
        renderView dispatch pos cell state.ServerMoving
    | _ ->
        renderView dispatch pos Blank state.ServerMoving


let private renderWin winner dispatch =
    match winner with
    | Some w ->
        Modal.modal [ Modal.IsActive true ] [
            Modal.background [ ] [ ]
            Modal.Card.card [ ] [
                Modal.Card.title [] [ str "There was a winner!" ]
                Modal.Card.body [] [
                    (sprintf "The winner is %s" (w |> playerToStr)) |> str
                ]
                Modal.Card.foot [] [
                    Button.button [
                        Button.Color IsSuccess
                        Button.OnClick (fun _ -> dispatch NewGame) ] [ str "New game?" ]
                ]
            ]
        ]
    | None ->
        p [] []

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
                  renderWin model.Winner dispatch
              ]

          Footer.footer [ ]
                [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]

