module Models

type Model = { Counter: int; ActiveIndex : int ResizeArray }

type Msg =
| Increment
| Decrement
| SetActiveIndex of int ResizeArray
