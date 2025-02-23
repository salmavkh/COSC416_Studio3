using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public int maxJumps = 2;
    private int jumpCount = 0;
    private bool isGrounded;

    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashCooldown = 2f;
    private bool canDash = true;

    private Rigidbody rb;
    private Vector3 lastMoveDirection = Vector3.zero; // Store last movement direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MovePlayer();

        // Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
        }

        // Dash Input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.y = 0;

        if (moveDirection.magnitude > 0.1f)
        {
            lastMoveDirection = moveDirection.normalized; // Store last valid movement direction
        }

        rb.AddForce(moveDirection * moveSpeed);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;

        if (jumpCount >= maxJumps)
        {
            isGrounded = false; // Prevent further jumps until landing
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            jumpCount = 0; // Reset jumps when touching the ground
        }
    }

    IEnumerator Dash()
    {
        canDash = false;

        // Determine Dash Direction
        Vector3 dashDirection = lastMoveDirection.magnitude > 0.1f ? lastMoveDirection : transform.forward;

        // Apply Dash
        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
