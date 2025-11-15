using UnityEngine;

public class InfiniteRoad : MonoBehaviour
{
    public float scrollSpeed = 3f;
    public float roadHeight = 10f;
    public Transform[] roadSegments;

    void Update()
    {
        if (RacingGameManager.RacingGameOver)
            return;

        foreach (Transform road in roadSegments)
        {
            road.position += Vector3.down * scrollSpeed * Time.deltaTime;

            if (road.position.y <= -roadHeight)
            {
                float newY = GetHighestRoadY() + roadHeight;
                road.position = new Vector3(road.position.x, newY, road.position.z);
            }
        }
    }

    float GetHighestRoadY()
    {
        float high = Mathf.NegativeInfinity;
        foreach (Transform road in roadSegments)
            if (road.position.y > high)
                high = road.position.y;
        return high;
    }
}