module Charts

open Models

open System
open Fable.Core
open Fable.Recharts
open Fable.Recharts.Props
open Fable.Core.JsInterop
module R = Fable.React.Standard
module P = Fable.React.Props
module F = Fable.React.Helpers

open Browser.Types

type PolarData = { name: string; value: int }

type IPolarPayload =
    abstract fill : string with get, set
    abstract name : string with get, set
    abstract strock : string with get, set
    abstract value : int with get, set

type IPolarProps =
    abstract midAngle : float with get, set
    abstract startAngle : float with get, set
    abstract endAngle : float with get, set
    abstract outerRadius : float with get, set
    abstract innerRadius : float with get, set
    abstract cx : float with get, set
    abstract cy : float with get, set
    abstract fill : string with get, set
    abstract payload : IPolarPayload with get, set
    abstract percent : float with get, set
    abstract value : float with get, set

type [<RequireQualifiedAccess>] Polar =
    | ActiveShape of (IPolarProps -> Fable.React.ReactElement)
    | OnClick of (IPolarProps -> int -> MouseEvent -> unit)
    interface Fable.React.Props.IProp

let margin t r b l =
    Chart.Margin { top = t; bottom = b; right = r; left = l }

let private onMouseEvent data evt = // should have sig (data, activeIndex, event)
  JS.console.log("MOUSE:", data, evt)

let private onPieClick (dispatch : Msg -> unit)  =
    let dis = dispatch
    fun (data : IPolarProps) index evt -> // should have sig (data, activeIndex, event)
        JS.console.log("MOUSE:", "Group " + string index, data, evt)
        let d = data
        if d.value < 500.0 then
            dis Increment
        else
            dis Decrement

let colors = ["#0088FE"; "#00C49F"; "#FFBB28"; "#FF8042"]

let private onPieEnter dispatch =
    let dis cmd =
        dispatch cmd
    fun _ index _ -> dis (SetActiveIndex index)

let polarDataA count =
    let positive = if count > 0 then count else 100
    let negative = if count < 0 then count * -1 else 100

    [| { name = "Positive"; value = positive }
       { name = "Negative"; value = negative }
    |]

let polarDataB =
    [| { name = "Group A"; value = 1000 }
       { name = "Group B"; value = 450 }
       { name = "Group C"; value = 600 }
    |]

let renderActiveShape (data : IPolarProps) =
    let radian = Math.PI / 180.;
    let sin = Math.Sin(-radian * data.midAngle)
    let cos = Math.Cos(-radian * data.midAngle)
    let sx = data.cx + (data.outerRadius + 10.) * cos
    let sy = data.cy + (data.outerRadius + 10.) * sin
    let mx = data.cx + (data.outerRadius + 30.) * cos
    let my = data.cy + (data.outerRadius + 30.) * sin
    let ex = mx + (if cos >= 0. then  1. else -1.) * 22.
    let ey = my
    let textAnchor = if cos >= 0. then "start" else "end";

    R.g [] [
        R.text [
            P.TextAnchor "middle"
            P.Fill data.fill
            P.X data.cx
            P.Y data.cy
            P.Dy 8
        ] [ F.str data.payload.name ]
        sector [
            Chart.Cx data.cx
            Chart.Cy data.cy
            Chart.InnerRadius data.innerRadius
            Polar.OuterRadius data.outerRadius
            Polar.StartAngle data.startAngle
            Polar.EndAngle data.endAngle
            P.Fill data.fill
        ]
        sector [
            Chart.Cx data.cx
            Chart.Cy data.cy
            Polar.StartAngle data.startAngle
            Polar.EndAngle data.endAngle
            Chart.InnerRadius (data.outerRadius + 6.)
            Chart.OuterRadius (data.outerRadius + 10.)
            P.Fill data.fill
        ]
        R.path [
            P.D (sprintf "M%f,%fL%f,%fL%f,%f" sx sy mx my ex ey)
            P.Stroke data.fill
            P.Fill "none"
        ] []
        R.circle [
            P.Cx ex
            P.Cy ey
            P.R 2
            P.Fill data.fill
            P.Stroke "none"
        ] []
        R.text [
            P.X  (ex + (if cos >= 0. then 1. else -1.) * 12.)
            P.Y (ey)
            P.TextAnchor textAnchor
            P.Fill "#333"
        ] [ F.str (sprintf "PV %.0f" data.value )]
        R.text [
            P.X  (ex + (if cos >= 0. then 1. else -1.) * 12.)
            P.Y (ey)
            P.TextAnchor textAnchor
            P.Fill "#999"
            P.Dy "18"
        ] [ F.str (sprintf "(Rate %.2f%%)" (data.percent * 100.))]
    ]

let pieChartSample (state : Model) dispatch =
    pieChart
        [ margin 10. 30. 0. 0.
          Chart.Width 800.
          Chart.Height 400.
        ] [
          pie [
            Polar.Data polarDataB
            Polar.DataKey "value"
            Polar.NameKey "name"
            Polar.Label false
            Polar.InnerRadius 60
            Polar.OuterRadius 80
            Polar.Cx "50%"
            Polar.Cy "50%"
            Polar.ActiveShape renderActiveShape
            Polar.ActiveIndex state.ActiveIndex
            Polar.OnClick (onPieClick dispatch)
            Polar.OnMouseEnter (onPieEnter dispatch)
            P.Fill "#8884d8"
          ] []
          pie [
            Polar.Data (polarDataA state.Counter)
            Polar.DataKey "value"
            Polar.NameKey "name"
            Polar.Label false
            Polar.InnerRadius 45
            Polar.OuterRadius 50
            Polar.Cx "50%"
            Polar.Cy "50%"
            P.Fill "#82ca9d"
            Cell.Fill (colors.[(abs state.Counter) % colors.Length])
          ] []
        ]
