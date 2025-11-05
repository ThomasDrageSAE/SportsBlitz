using UnityEngine;
using System.Collections;

public class CameraZoomToTV : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0.5f, 0, 0);
    public float moveSpeed = 2f;
    public float targetSize = 2f;
    public float delayBeforeSwitch = 1f;

    Camera cam;
    Vector3 startPos;
    float startSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        startPos = transform.position;
        startSize = cam.orthographicSize;
    }

    public void StartZoom()
    {
        StartCoroutine(ZoomToTV());
    }

    public void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomBack());
    }

    IEnumerator ZoomToTV()
    {
        Vector3 zoomPos = target.position + offset;
        zoomPos.z = startPos.z;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            transform.position = Vector3.Lerp(startPos, zoomPos, t);
            yield return null;
        }
    }

    IEnumerator ZoomBack()
    {
        float t = 0;
        Vector3 zoomPos = target.position + offset;
        zoomPos.z = startPos.z;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            cam.orthographicSize = Mathf.Lerp(targetSize, startSize, t);
            transform.position = Vector3.Lerp(zoomPos, startPos, t);
            yield return null;
        }
    }
}
