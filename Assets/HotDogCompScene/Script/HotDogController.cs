using UnityEngine;

public class HotDogController : MonoBehaviour
{
    int eatCount = 0;
    bool gameState = false;
    int scoreToBeat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreToBeat = Random.Range(35, 46);
        
        Debug.Log(scoreToBeat);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameState == true)
        {
            eatCount ++;
            Debug.Log(eatCount);
        }
    }

    


}
