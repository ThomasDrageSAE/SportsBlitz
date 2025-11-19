using UnityEngine;
using Helper.Blake;

public class Hurdles : MonoBehaviour
{
    // Movement
    public float moveSpeed = 5f;
    public float jumpHeight = 7f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Listen for correct key from InputManager
        EventManager.Instance.correctKeyInput += OnCorrectKeyPressed;
    }

    private void OnDisable()
    {
        EventManager.Instance.correctKeyInput -= OnCorrectKeyPressed;
    }

    private void FixedUpdate()
    {
        // Constant forward movement
        Vector3 forwardMovement = Vector3.forward * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }

    private void OnCorrectKeyPressed(string key)
    {
        // Jump when correct key is pressed
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // Check if player is on ground
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}