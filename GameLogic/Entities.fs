namespace GameLogic.Entities

open UnityEngine

type BuildProgress
    = Done
    | Foundation
    | InProgress of float32

type IGlobalState =
    abstract selectBuilder : IBuilder -> unit
    abstract selectAnotherBuilder : IBuilder -> unit
    abstract selectBuilders : IBuilder list -> unit
    abstract dispatchBuild : IStructure -> unit
    abstract dispatchRepair : IStructure -> unit
    abstract dispatchMove : Vector2 -> unit
    abstract dispatchMove : Vector3 -> unit
    abstract dispatchMoneyPickup : string -> Vector3 -> unit
    abstract giveMoney : int16 -> unit
    abstract takeMoney : int16 -> unit
    abstract currentMoney : int16
    abstract getBuilders : IBuilder list

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
    abstract setRepairTarget : IStructure -> unit
    abstract setMoveTarget : Vector3 -> unit
    abstract setMoneyPickupTarget : string -> Vector3 -> unit
