using UnityEngine;

public class IdleState : IState
{
    private readonly Animator _animator;
    private readonly Transform[] _patrolPoints;
    private readonly CharacterController _controller;
    private readonly float _patrolSpeed;
    private int _nextPatrolPointIndex = 0;

    public IdleState(Animator animator, Transform[] patrolPoints, CharacterController controller, float patrolSpeed)
    {
        _animator = animator;
        _patrolPoints = patrolPoints;
        _controller = controller;
        _patrolSpeed = patrolSpeed;
    }

    public void Enter()
    {
        _animator.SetBool("Running", false);
    }

    public void Execute()
    {
        Vector3 target = _patrolPoints[_nextPatrolPointIndex].position;
        Vector3 direction = (target - _controller.transform.position).normalized;
        if (Vector3.Distance(_controller.transform.position, target) > 1f)
        {
            _controller.SimpleMove(direction * _patrolSpeed);
            _animator.SetBool("Running", true);
        }
        else
        {
            _nextPatrolPointIndex = (_nextPatrolPointIndex + 1) % _patrolPoints.Length;
            _animator.SetBool("Running", false);
        }
    }

    public void Exit()
    {
        // Exit logic for IdleState if needed
    }
}
