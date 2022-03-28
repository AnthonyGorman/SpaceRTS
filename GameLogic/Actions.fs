namespace GameLogic

open System
open UnityEngine

open GameLogic.Entities

module BuilderLogic =
    let zeroVector = Vector2(0f,0f)
    let zeroVector3 = Vector3(0f,0f,0f)

    type BuildState
        = MovingToBuild of IStructure
        | Building of IStructure

    type RepairState
        = MovingToRepair of IStructure
        | Repairing of IStructure

    type Action
        = Idle
        | MovingTo of Vector3
        | Build of BuildState
        | Repair of RepairState
        | PickupMoney of string * Vector3

    let displayCurrentAction (action : Action) : string =
        match action with
        | Idle -> "Idle"
        | MovingTo pos -> $"Moving to: {pos.x}, {pos.y}"
        | Build s -> $"Building: " + (match s with
                                        | MovingToBuild st -> $"Moving to {st.name} ({st.position.x}, {st.position.y})"
                                        | Building st -> $"Building {st.name}")
        | Repair r -> $"Repairing: " + (match r with
                                        | MovingToRepair st -> $"Moving to {st.name} ({st.position.x}, {st.position.y})"
                                        | Repairing st -> $"Repairing {st.name}")
        | PickupMoney (name, pos) -> $"Picking up money from {pos.x}, {pos.y}"

    let getNewBuildProgress (structure : IStructure) : BuildProgress * float32 =
        match structure.buildProgress with
        | Done -> (Done, min structure.health structure.maxHealth)
        | Foundation -> (InProgress 0.0f, structure.maxHealth / 2.0f)
        | InProgress progress -> if progress > 1.0f
                                    then (Done, min structure.maxHealth structure.health)
                                    else ( structure.buildSpeed + progress |> InProgress
                                         , structure.health + structure.buildSpeed * 0.5f * structure.maxHealth
                                         )

    let moveTo (pos : Vector3) (entity : IMovableEntity) : bool =
        if (pos - entity.position).magnitude < 0.01f
            then true
            else
                (pos - entity.position).normalized * entity.speed
                    |> entity.setVelocity
                false

    let handleCollision (state : Action) (builder : IBuilder) (targetName : string) : Action =
        match state with
        | Idle -> Idle
        | MovingTo pos -> MovingTo pos
        | Build buildState ->
            (match buildState with
            | MovingToBuild structure ->
                if structure.name = targetName
                    then builder.setVelocity zeroVector3
                         Building structure
                    else MovingToBuild structure
            | Building structure -> Building structure)
                |> Build
        | Repair repairState ->
            (match repairState with
            | MovingToRepair structure ->
                if structure.name = targetName
                    then builder.setVelocity zeroVector3
                         Repairing structure
                    else MovingToRepair structure
            | Repairing structure -> Repairing structure)
                |> Repair
        | PickupMoney (name, pos) ->
            if name = targetName
                then builder.globalState.giveMoney 50s
                     Idle
                else PickupMoney (name, pos)

    let handleCollisionLeave (state : Action) (builder : IBuilder) (targetName : string) : Action =
        match state with
        | Idle -> Idle
        | MovingTo pos -> MovingTo pos
        | Build buildState ->
            (match buildState with
            | MovingToBuild structure -> MovingToBuild structure
            | Building structure -> 
                if structure.name = targetName
                    then MovingToBuild structure
                    else Building structure)
                |> Build
        | Repair repairState ->
            (match repairState with
            | MovingToRepair structure -> MovingToRepair structure
            | Repairing structure -> 
                if structure.name = targetName
                    then MovingToRepair structure
                    else Repairing structure)
                |> Repair
        | PickupMoney (name, pos) -> PickupMoney (name, pos)

    let applyBuilderAction (state : Action) (builder : IBuilder) : Action =
        match state with
        | Idle -> Idle

        | MovingTo pos ->
            if moveTo pos builder
                then builder.setVelocity Vector3.zero
                     Idle
                else MovingTo pos

        | PickupMoney (name, pos) ->
            if moveTo pos builder
                then builder.setVelocity Vector3.zero
                     Idle
                else PickupMoney (name, pos)

        | Build buildState ->
            match buildState with
            | MovingToBuild structure ->
                (structure.position - builder.position).normalized * builder.speed
                    |> builder.setVelocity
                MovingToBuild structure |> Build
            | Building structure ->
                match structure.buildProgress with
                | Foundation ->
                     let (progress, health) = getNewBuildProgress structure
                     structure.setBuildProgress progress
                     structure.setHealth health
                     Building structure |> Build
                | InProgress progress ->
                    if progress > 1.0f
                        then structure.setBuildProgress Done
                             Idle
                        else let (progress, health) = getNewBuildProgress structure
                             structure.setBuildProgress progress
                             structure.setHealth health
                             Building structure |> Build
                | Done -> Idle

        | Repair repairState ->
            match repairState with
            | MovingToRepair structure ->
                (structure.position - builder.position).normalized * builder.speed
                    |> builder.setVelocity
                MovingToRepair structure |> Repair
            | Repairing structure ->
                if structure.health >= structure.maxHealth
                    then Idle
                    else structure.health + structure.buildSpeed * 0.5f * structure.maxHealth
                            |> structure.setHealth
                         Repairing structure |> Repair

    let handleMouseDown (builder : IBuilder) =
        if Input.GetKey KeyCode.LeftShift
            then builder.globalState.selectAnotherBuilder builder
            else builder.globalState.selectBuilder builder

module StructureLogic =
    let handleMouseOver (structure : IStructure) =
        if Input.GetMouseButtonDown 1
            then match structure.buildProgress with
                 | Done -> if structure.health < structure.maxHealth
                               then structure.globalState.dispatchRepair structure
                 | InProgress _ -> structure.globalState.dispatchBuild structure
                 | Foundation -> structure.globalState.dispatchBuild structure

module GameScale =
    let handleUpdate (globalState : IGlobalState) =
        if Input.GetMouseButtonDown 1 
           then let mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition)
                               |> Vector2.op_Implicit
                let hit = Physics2D.Raycast(mousePos, Vector2.zero)
                if hit.collider = null
                   then globalState.dispatchMove mousePos

        if Input.GetMouseButtonDown 0
           then let mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition)
                               |> Vector2.op_Implicit
                let hit = Physics2D.Raycast(mousePos, Vector2.zero)
                if hit.collider = null
                   then globalState.selectBuilders []

module ButtonLogic =
    let shouldButtonBeEnabled (purchaseCost : int16) (globalState : IGlobalState) : bool =
        globalState.currentMoney > purchaseCost && globalState.getBuilders.Length > 0

module MoneyLogic =
    let handleMouseOver (name : string) (position : Vector3) (globalState : IGlobalState) =
        if Input.GetMouseButtonDown 1 then
            globalState.dispatchMoneyPickup name position

    let handleCollision (obj : MonoBehaviour) (globalState : IGlobalState) =
        globalState.giveMoney 50s
        Object.Destroy obj.gameObject

