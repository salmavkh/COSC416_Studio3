using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;
    public Transform cameraTransform; // Reference to the Cinemachine camera

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void MovePlayer()
    {
        Vector2 inputVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputVector += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += Vector2.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += Vector2.left;
        }

        // Convert input to 3D movement based on camera direction
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        moveDirection = cameraTransform.forward * moveDirection.z + cameraTransform.right * moveDirection.x;
        moveDirection.y = 0; // Keep movement on the ground plane

        rb.AddForce(moveDirection * moveSpeed, ForceMode.Force);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }
}
