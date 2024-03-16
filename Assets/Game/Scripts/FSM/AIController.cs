using UnityEngine;

public class AIController : MonoBehaviour
{
    private IState _currentState;
    private ChaseState _chaseState;
    private DanceState _danceState;
    private IdleState _idleState;

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _chaseDistance = 10f;
    [SerializeField] private float _danceDistance = 2f;

    private void Awake()
    {
        _chaseState = new ChaseState(_animator, _playerTransform, _controller, 5f); // Assume chase speed is 5
        _danceState = new DanceState(_animator, 2.1f); // Assume dance duration is 2.1 seconds
        _idleState = new IdleState(_animator, _patrolPoints, _controller, 2f); // Assume patrol speed is 2
        _currentState = _idleState;
        _currentState.Enter();
    }

    private void Update()
    {
        _currentState.Execute();
        CheckTransitions();
    }

    private void CheckTransitions()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (_currentState == _idleState && distanceToPlayer < _chaseDistance)
        {
            ChangeState(_chaseState);
        }
        else if (_currentState == _chaseState && distanceToPlayer <= _danceDistance)
        {
            ChangeState(_danceState);
        }
        else if (_currentState == _danceState && _danceState.IsDanceComplete())
        {
            ChangeState(_idleState);
        }
    }

    private void ChangeState(IState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
