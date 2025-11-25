using UnityEngine;
using UnityEngine.UIElements;

public class WrestlingArrowTurning : MonoBehaviour
{
    [SerializeField] float rotationAmount;
    [SerializeField] WrestlingManager manager;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (manager.gameStart == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z - (rotationAmount * Time.deltaTime), transform.rotation.w);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + (rotationAmount * Time.deltaTime), transform.rotation.w);
            }
        }
    }
}
