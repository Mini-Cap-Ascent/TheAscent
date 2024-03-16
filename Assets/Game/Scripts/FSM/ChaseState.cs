using UnityEngine;

public class ChaseState : IState
{
    private readonly Animator _animator;
    private readonly Transform _playerTransform;
    private readonly CharacterController _controller;
    private readonly float _chaseSpeed;

    public ChaseState(Animator animator, Transform playerTransform, CharacterController controller, float chaseSpeed)
    {
        _animator = animator;
        _playerTransform = playerTransform;
        _controller = controller;
        _chaseSpeed = chaseSpeed;
    }

    public void Enter()
    {
        _animator.SetBool("Running", true);
    }

    public void Execute()
    {
        Vector3 direction = (_playerTransform.position - _controller.transform.position).normalized;
        direction.y = 0; // Keep the movement horizontal
        _controller.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        _controller.SimpleMove(direction * _chaseSpeed); // SimpleMove automatically applies deltaTime
    }

    public void Exit()
    {
        _animator.SetBool("Running", false);
    }
}
