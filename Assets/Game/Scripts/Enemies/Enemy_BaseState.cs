using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_BaseState : FSMBaseState<Enemy_FSM>
{
  
    protected Enemy_Controller enemyController;
    protected NavMeshAgent navMeshAgent;
    protected Transform playerTransform;
    protected Animator enemyAnimator;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        enemyController = _owner.GetComponent<Enemy_Controller>();
        Debug.Assert(enemyController != null, "Enemy_Controller not found");

        navMeshAgent = _owner.GetComponent<NavMeshAgent>();
        Debug.Assert(navMeshAgent != null, "NavMeshAgent not found");
        
        playerTransform = owner.GetComponent<Transform>();
        Debug.Assert(playerTransform != null, "Player Transform not found");

        enemyAnimator = _owner.GetComponent<Animator>();
        Debug.Assert(enemyAnimator != null, "Enemy Animator not found");

    }


}
