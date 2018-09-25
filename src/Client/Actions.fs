namespace Actions

open Shared

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
| PlayMove of Position
| InitialGameLoaded of Result<Board, exn>
| ServerMove of Result<Position, exn>

