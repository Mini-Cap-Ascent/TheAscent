namespace Fusion
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Explicit)]
    [NetworkStructWeaved(WORDS + 4)]
    public unsafe struct NetworkCCData : INetworkStruct
    {
        public const int WORDS = NetworkTRSPData.WORDS + 4;
        public const int SIZE = WORDS * 4;

        [FieldOffset(0)]
        public NetworkTRSPData TRSPData;

        [FieldOffset((NetworkTRSPData.WORDS + 0) * Allocator.REPLICATE_WORD_SIZE)]
        int _grounded;

        [FieldOffset((NetworkTRSPData.WORDS + 1) * Allocator.REPLICATE_WORD_SIZE)]
        Vector3Compressed _velocityData;

        public bool Grounded
        {
            get => _grounded == 1;
            set => _grounded = (value ? 1 : 0);
        }

        public Vector3 Velocity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _velocityData;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _velocityData = value;
        }
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [NetworkBehaviourWeaved(NetworkCCData.WORDS)]
    // ReSharper disable once CheckNamespace
    public sealed unsafe class NetworkCharacterController : NetworkTRSP, INetworkTRSPTeleport, IBeforeAllTicks, IAfterAllTicks, IBeforeCopyPreviousState
    {
        new ref NetworkCCData Data => ref ReinterpretState<NetworkCCData>();

        [Header("Character Controller Settings")]
        public float gravity = -20.0f;
        public float jumpImpulse = 8.0f;
        public float acceleration = 10.0f;
        public float braking = 10.0f;
        public float maxSpeed = 2.0f;
        public float rotationSpeed = 15.0f;
        private Animator _animator;
        private bool _wantsToJump = false;
        public bool IsGrounded { get; private set; }
        Tick _initial;
        CharacterController _controller;

        void Awake()
        {
            TryGetComponent(out _controller);
            TryGetComponent(out _animator); // Get the Animator component
        }

        public Vector3 Velocity
        {
            get => Data.Velocity;
            set => Data.Velocity = value;
        }

        public bool Grounded
        {
            get => Data.Grounded;
            set => Data.Grounded = value;
        }
        public void RequestJump()
        {
            if (IsGrounded)
            {
                IsGrounded = false; // Immediately set to false to prevent double jumps
                Velocity = new Vector3(Velocity.x, jumpImpulse, Velocity.z);
            }
        }

        public void Teleport(Vector3? position = null, Quaternion? rotation = null)
        {
            _controller.enabled = false;
            NetworkTRSP.Teleport(this, transform, position, rotation);
            _controller.enabled = true;
        }

        public void MovingAttack()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("MAttack");
            }
        }

        // Call this method to perform an idle attack
        public void IdleAttack()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("IAttack");
            }
        }

        public void Jump()
        {
            if (Grounded)
            {
                Vector3 jumpVelocity = new Vector3(0f, jumpImpulse, 0f);
                Velocity += jumpVelocity; // Add the jump impulse to the current velocity
                Grounded = false; // The character is now in the air
            }
        }

        public void HandleInput()
        {
            // Check if the player has pressed the number '1' key to attack
            bool isAttacking = Input.GetKeyDown(KeyCode.Keypad1);

            // Check if the player is moving by evaluating the magnitude of the velocity vector
            // You could also use input axis values if you want to check for movement input instead
            bool isMoving = Velocity.magnitude > 0;

            if (isAttacking)
            {
                if (isMoving)
                {
                    MovingAttack();
                }
                else
                {
                    IdleAttack();
                }
            }
        }
        public void Move(Vector3 direction)
        {
            var deltaTime = Runner.DeltaTime;
            var previousPos = transform.position;
            var moveVelocity = Data.Velocity;

            direction = direction.normalized;

            if (Data.Grounded && moveVelocity.y < 0)
            {
                moveVelocity.y = 0f;
            }

            moveVelocity.y += gravity * Runner.DeltaTime;

            var horizontalVel = default(Vector3);
            horizontalVel.x = moveVelocity.x;
            horizontalVel.z = moveVelocity.z;
            CheckIfGrounded();
            if (_wantsToJump)
            {
                moveVelocity.y = jumpImpulse; // Set the vertical velocity to the jump impulse
                _wantsToJump = false;
            }
            else
            {
                // Apply gravity
                moveVelocity.y += gravity * Runner.DeltaTime;
            }

            if (direction == default)
            {
                horizontalVel = Vector3.zero;
                //horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
            }
            else
            {
                horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
                //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Runner.DeltaTime);
            }

            moveVelocity.x = horizontalVel.x;
            moveVelocity.z = horizontalVel.z;

            _controller.Move(moveVelocity * deltaTime);

            Data.Velocity = (transform.position - previousPos) * Runner.TickRate;
            Data.Grounded = _controller.isGrounded;
        }
        private void CheckIfGrounded()
        {
            // Cast a ray downwards to check for ground
            RaycastHit hit;
            float raycastDistance = 0.1f; // Adjust the distance based on your character's setup
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, _controller.height / 2f + raycastDistance))
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }
        public override void Spawned()
        {
            _initial = default;
            TryGetComponent(out _controller);
            CopyToBuffer();
        }

        public override void Render()
        {
            NetworkTRSP.Render(this, transform, false, false, false, ref _initial);
        }

        void IBeforeAllTicks.BeforeAllTicks(bool resimulation, int tickCount)
        {
            CopyToEngine();
        }

        void IAfterAllTicks.AfterAllTicks(bool resimulation, int tickCount)
        {
            CopyToBuffer();
        }

        void IBeforeCopyPreviousState.BeforeCopyPreviousState()
        {
            CopyToBuffer();
        }



        void CopyToBuffer()
        {
            Data.TRSPData.Position = transform.position;
            Data.TRSPData.Rotation = transform.rotation;
        }

        void CopyToEngine()
        {
            // CC must be disabled before resetting the transform state
            _controller.enabled = false;

            // set position and rotation
            transform.SetPositionAndRotation(Data.TRSPData.Position, Data.TRSPData.Rotation);

            // Re-enable CC
            _controller.enabled = true;
        }
        public override void FixedUpdateNetwork()
        {
            // Call the Move method which handles jump logic as well
            Move(Data.Velocity * Runner.DeltaTime);
        }
    }
}