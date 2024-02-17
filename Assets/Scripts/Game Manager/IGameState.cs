using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    void EnterState();
    void UpdateState();
    void ExitState();
    void ResumeState(); // Add this method if you're using the state history stack approach
}