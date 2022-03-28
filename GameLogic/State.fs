namespace GameLogic.State

open System
open UnityEngine

open GameLogic.Entities

type GameStateWrapper() as this =
    inherit MonoBehaviour()

    [<NonSerialized()>]
    [<HideInInspector>]
    [<DefaultValue>]
    val mutable currentlySelectedBuilders : IBuilder list

    [<SerializeField>]
    [<DefaultValue>]
    val mutable currentBalance : int16

    do
        this.currentlySelectedBuilders <- []
        this.currentBalance <- 0s

    interface IGlobalState with
        member this.selectBuilder(builder : IBuilder) =
            for builder in this.currentlySelectedBuilders do
                builder.selected <- false

            this.currentlySelectedBuilders <- [builder]
            builder.selected <- true

        member this.selectAnotherBuilder(builder : IBuilder) =
            this.currentlySelectedBuilders <- builder :: this.currentlySelectedBuilders
            builder.selected <- true

        member this.selectBuilders(builders : IBuilder list) =
            for builder in this.currentlySelectedBuilders do
                builder.selected <- false
            this.currentlySelectedBuilders <- builders
            for builder in this.currentlySelectedBuilders do
                builder.selected <- true

        member this.dispatchBuild(structure : IStructure) =
            for builder in this.currentlySelectedBuilders do
                builder.setBuildTarget structure

        member this.dispatchRepair(structure : IStructure) =
            for builder in this.currentlySelectedBuilders do
                builder.setRepairTarget structure

        member this.dispatchMove(position : Vector3) =
            for builder in this.currentlySelectedBuilders do
                builder.setMoveTarget position

        member this.dispatchMove(position : Vector2) =
            for builder in this.currentlySelectedBuilders do
                position
                    |> Vector2.op_Implicit
                    |> builder.setMoveTarget

        member this.dispatchMoneyPickup (name : string) (position : Vector3) =
            for builder in this.currentlySelectedBuilders do
                builder.setMoneyPickupTarget name position

        member this.takeMoney(amount : int16) =
            this.currentBalance <- this.currentBalance - amount

        member this.giveMoney(amount : int16) =
            this.currentBalance <- this.currentBalance - amount

        member this.currentMoney = this.currentBalance

        member this.getBuilders = this.currentlySelectedBuilders
