using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float maxDrag = 4f;
    public float power = 8f;
    public Rigidbody2D rb;
    public LineRenderer lr;

    Vector3 dragStartPosition;
    bool dragging = false;

    // Update is called once per frame
    void Update()
    {
        if(dragging == true)
        {
            Vector3 draggingPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            draggingPosition.z = 0;

            Vector3 finalDraggingPosition = 2 * dragStartPosition - draggingPosition;

            lr.positionCount = 2;
            lr.SetPosition(1, finalDraggingPosition);
        }

        if(dragging == false && Input.GetMouseButtonDown(0))
        {
            // Get starting Dragging position
            dragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragStartPosition.z = 0; // This is set to zero to prevent z axis rotation

            dragging = true;

            lr.positionCount = 1;
            lr.SetPosition(0, dragStartPosition);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lr.positionCount = 0;
            dragging = false;

            // Apply force to the ball
            Vector3 dragReleasePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 force = dragStartPosition - dragReleasePosition;
            Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power * 2;

            if (rb.linearVelocity.magnitude >= 0 && rb.linearVelocity.magnitude <= 0.5f)
            {
                rb.AddForce(clampedForce, (ForceMode2D)ForceMode2D.Impulse);
            }
        }
    }
}
