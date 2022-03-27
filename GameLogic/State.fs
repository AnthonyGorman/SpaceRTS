namespace GameLogic.State

open System.Collections.Generic
open UnityEngine

open GameLogic.Entities

type GameStateWrapper() =
    inherit MonoBehaviour()

    let mutable currentlySelectedBuilders : List<IBuilder> = List<IBuilder>();

    interface IGlobalState with
        member this.dummy = 3f

    member this.selectBuilder(builder : IBuilder) =
        for builder in currentlySelectedBuilders do
            builder.selected <- false

        currentlySelectedBuilders <- List()
        currentlySelectedBuilders.Add(builder)
        builder.selected <- true

    member this.selectAnotherBuilder(builder : IBuilder) =
        currentlySelectedBuilders.Add(builder)
        builder.selected <- true

    member this.selectBuilders(builders : List<IBuilder>) =
        for builder in currentlySelectedBuilders do
            builder.selected <- false
        currentlySelectedBuilders <- builders
        for builder in currentlySelectedBuilders do
            builder.selected <- true

    member this.dispatchBuild(structure : IStructure) =
        for builder in currentlySelectedBuilders do
            builder.setBuildTarget structure

    member this.getBuilders =
        currentlySelectedBuilders
