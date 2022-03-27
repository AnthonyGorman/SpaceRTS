namespace GameLogic.Entities

open System
open UnityEngine

type BuildProgress
    = Done
    | Foundation
    | InProgress of float32

type IGlobalState =
    // abstract selectBuilder : IBuilder -> unit
    // abstract selectAnotherBuilder : IBuilder -> unit
    // abstract selectBuilders : IBuilder array -> unit
    // abstract dispatchBuild : IStructure -> unit
    abstract dummy : float32

and IEntity =
    abstract globalState : IGlobalState
    abstract name : string
    abstract position : Vector3
    abstract rigidBody : Rigidbody2D

and IEntityWithHealth =
    inherit IEntity

    abstract maxHealth : float32
    abstract health : float32
    abstract setHealth : float32 -> unit

and IStructure =
    inherit IEntityWithHealth

    abstract buildSpeed : float32
    abstract buildProgress : BuildProgress
    abstract setBuildProgress : BuildProgress -> unit

and IMovableEntity =
    inherit IEntity

    abstract velocity : Vector3
    abstract speed : float32
    abstract setVelocity : Vector3 -> unit

and IEnemy =
    inherit IMovableEntity
    inherit IEntityWithHealth

    abstract attackStrength : float32
    abstract attackRange : float32

and IBuilder =
    inherit IMovableEntity
    inherit IEntityWithHealth

    abstract selected : bool with get, set
    abstract setBuildTarget : IStructure -> unit
