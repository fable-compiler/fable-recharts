module Models

type Model = { Counter: int; ActiveIndex : int }

type Msg =
| Increment
| Decrement
| SetActiveIndex of int
