using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public abstract class SteeringBehaviourBase : MonoBehaviour
{
    public float weight = 1.0f;
    public Vector3 target = Vector3.zero;

    public abstract Vector3 CalculateForce();
    

    [HideInInspector] public SteeringAgent steeringAgent;

    public bool useMouseInput = true;
    protected bool mouseClicked= false;


    protected void CheckMouseInput()
    {
        mouseClicked = false;
        if(Input.GetMouseButtonDown(0)&&useMouseInput) 
        { 
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit,100)) 
            {
                target=hit.point;
                mouseClicked = true;
            }
        }
    }


}
