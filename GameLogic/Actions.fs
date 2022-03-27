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
                (entity.position - pos).normalized * entity.speed
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

    let applyBuilderAction (state : Action) (builder : IBuilder) : Action =
        match state with
        | Idle -> Idle

        | MovingTo pos ->
            if moveTo pos builder
                then Idle
                else MovingTo pos

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

