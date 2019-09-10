module App

open Elmish
open Elmish.React
open Fable.Core
open Fable.React
open Fable.React.Props

open Models
open Charts

let init() : Model  = { Counter = 0; ActiveIndex = [0;2] |> ResizeArray }

// UPDATE

let update (msg:Msg) (model:Model) =
    match msg with
    | Increment -> { model with Counter = model.Counter + 1 }
    | Decrement -> { model with Counter = model.Counter - 1 }
    | SetActiveIndex index -> { model with ActiveIndex = index }

// VIEW (rendered with React)

let mkButton txt onClick =
    button [ Style [ Width "100px"; Margin "0 auto" ]
             OnClick (fun _ -> onClick()) ]
           [ str txt ]

let view (model:Model) dispatch =
  div [] [
    div [] [ pieChartSample model dispatch]
    div [ Style [ Width "800px"; Display DisplayOptions.InlineGrid]]
        [ span [ Style [TextAlign TextAlignOptions.Center] ] [str (sprintf "Counter: %i" model.Counter)]
          mkButton "+" (fun _ -> dispatch Increment) 
          mkButton "-" (fun _ -> dispatch Decrement) ] ]

// App
Program.mkSimple init update view
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run
