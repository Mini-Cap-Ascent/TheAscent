using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    private PlayerCont controls;
    private Vector2 move;
    private Rigidbody rb;

/*    public GameObject cameraTarget;*/ // Assign this in the inspector
    public float rotationSpeed = 5f; // Adjust as needed

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerCont();

        controls.PlayerControlz.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.PlayerControlz.Move.canceled += ctx => move = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(move.x, 0, move.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        //// Update camera target's position to player's position
        //if (cameraTarget != null)
        //{
        //    cameraTarget.transform.position = rb.position;
        //    if (move != Vector2.zero)
        //    {
        //        // Calculate the rotation towards the movement direction
        //        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
        //        // Smoothly rotate the camera target towards the calculated rotation
        //        cameraTarget.transform.rotation = Quaternion.Slerp(cameraTarget.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        //    }
        //}
    }
}