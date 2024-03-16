using UnityEngine;

public class DanceState : IState
{
    private readonly Animator _animator;
    private float _danceTime;
    private float _danceTimer;

    public DanceState(Animator animator, float danceTime)
    {
        _animator = animator;
        _danceTime = danceTime;
    }

    public void Enter()
    {
        _animator.SetTrigger("Dance");
        _danceTimer = 0f;
    }

    public void Execute()
    {
        if (_danceTimer < _danceTime)
        {
            _danceTimer += Time.deltaTime;
        }
    }

    public void Exit()
    {
        _animator.ResetTrigger("Dance");
    }

    public bool IsDanceComplete()
    {
        return _danceTimer >= _danceTime;
    }
}
