using UnityEngine;

public class GolfGameController : MonoBehaviour
{
    public Rigidbody2D golfBall;
    public float maxPower = 20f;
    public float powerChargeSpeed = 10f;
    public Transform aimPivot;

    private float currentPower;
    private bool charging = false;

    private void Update()
    {
        HandleAiming();
        HandleCharging();
    }

    void HandleAiming()
    {
        //Rotate aim from Up and Down.
        if (Input.GetKey(KeyCode.W))
        {
            aimPivot.Rotate(Vector3.forward, 80f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            aimPivot.Rotate(Vector3.forward, -80f * Time.deltaTime);
        }
    }

    void HandleCharging()
    {
        // Begin charging Golf Swing.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            charging = true;
        }

        // Increase power of Golf Swing while holding Space Key.
        if(charging && Input.GetKey(KeyCode.Space))
        {
            currentPower += powerChargeSpeed * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower);
            Debug.Log("Power: " + currentPower);
        }

        // Release Golf Swing!
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShootBall();
            currentPower = 20f;
            charging = false;
        }
    }

    void ShootBall()
    {
        Vector2 hitDirection = aimPivot.right;
        golfBall.AddForce(hitDirection, ForceMode2D.Impulse);
        Debug.Log("Ball Hit!");
    }
}
