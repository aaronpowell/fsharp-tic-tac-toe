module Win

open Models

let private test count moves =
    moves
    |> Seq.filter (fun (_, items) -> (items |> Seq.length) = count)
    |> Seq.filter (
        fun (_, items) ->
            let d = items
                  |> Seq.map (
                        fun (_, player) -> player
                    )
                  |> Seq.distinct
            (d |> Seq.length) = 1)

let isWin (model: Model) =
    let fullCol = model.Board
                     |> Map.toSeq
                     |> Seq.groupBy (fun ((col, _), _) -> col)
                     |> test (model.Cols |> List.length)

    match fullCol |> Seq.isEmpty with
    | false ->
        let (_, pos) = fullCol |> Seq.head
        let (_, player) = pos |> Seq.head
        Some(pos, player)
    | true ->
        let fullRow = model.Board
                    |> Map.toSeq
                    |> Seq.groupBy (fun ((_, row), _) -> row)
                    |> test (model.Rows |> List.length)

        match fullRow |> Seq.isEmpty with
        | false ->
            let (_, pos) = fullRow |> Seq.head
            let (_, player) = pos |> Seq.head
            Some(pos, player)
        | true ->
            None