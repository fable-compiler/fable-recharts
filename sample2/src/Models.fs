module Models

type Model = { Counter: int; ActiveIndex : System.Array }

type Msg =
| Increment
| Decrement
| SetActiveIndex of System.Array
