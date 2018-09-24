namespace Shared

type Position = char * int
type Board = Map<Position, char>
type Model =
    { Rows : int list
      Cols : char list
      Board: Board }