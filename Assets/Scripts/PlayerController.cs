using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // Speed of turning

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
    private Vector3 lastMoveDirection = Vector3.zero;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

        // Convert movement direction to world space using camera orientation
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.y = 0; // Keep movement horizontal

        if (moveDirection.magnitude > 0.1f) // Ensure movement is happening
        {
            lastMoveDirection = moveDirection.normalized; // Store last valid movement direction

            // Rotate deer smoothly to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isWalking", true);  // Play walk animation
        }
        else
        {
            animator.SetBool("isWalking", false); // Stop walk animation
        }

        rb.AddForce(moveDirection * moveSpeed);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;

        animator.SetTrigger("Jump"); // Play jump animation

        if (jumpCount >= maxJumps)
        {
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;

        Vector3 dashDirection = lastMoveDirection.magnitude > 0.1f ? lastMoveDirection : transform.forward;

        animator.SetTrigger("Dash"); // Play dash animation

        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}