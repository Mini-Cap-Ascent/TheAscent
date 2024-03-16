using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent( typeof(Enemy_Controller))]
public class Enemy_FSM : FSM
{
    public readonly string JogStateName = "Jog";
    public readonly string PatrolStateName = "Patroling";
    public readonly string PlayerSpottedStateName = "PlayerSpotted";
    public readonly string DeathStateName = "Death";
}
