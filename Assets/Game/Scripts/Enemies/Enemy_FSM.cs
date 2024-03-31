using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent( typeof(NavMeshAgent), typeof(Enemy_Controller))]
public class Enemy_FSM : FSM
{
    public readonly string PatrolStateName = "Patroling";
    public readonly string EnemyFoundStateName = "EnemyFound";
    public readonly string HitStateName = "Hit";
    public readonly string DeathStateName = "Death";
}
