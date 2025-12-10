using System.Collections;
using UnityEngine;

public class HotDogTimer : MonoBehaviour
{
    float countdownTime = 6;

    public HotDogController hotDog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(6);
        CountdownFinished();
    }

    void CountdownFinished()
    {
        Debug.Log("Game Done");
        hotDog.GameOver();
    }
}
