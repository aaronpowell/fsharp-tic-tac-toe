module Models

type Position = char * int
type Player =
    | X
    | O
    | Blank
type Board = Map<Position, Player>
type Model =
    { Rows : int list
      Cols : char list
      Board : Board
      ServerMoving : bool
      Winner : Option<Player>
      WinningMoves : Option<seq<Position>> }
