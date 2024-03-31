using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGizmos : MonoBehaviour
{
    private Enemy_Patrol_FSM enemyPatrolFSM; // Assign this in the inspector

    private void Start()
    {
        if (enemyPatrolFSM == null)
        {
            enemyPatrolFSM = GetComponent<Enemy_Patrol_FSM>();
        }
    }
    

    void OnDrawGizmosSelected()
    {
        if (enemyPatrolFSM != null)
        {
            float eyeLevel = 1.8f; // Adjust as necessary
            Vector3 gizmoCenter = transform.position + Vector3.up * eyeLevel;

            // Draw the detection range sphere
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(gizmoCenter, enemyPatrolFSM.detectionRange);
        }
    }
}
