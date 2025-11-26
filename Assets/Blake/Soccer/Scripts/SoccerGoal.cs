using UnityEngine;

public class SoccerGoal : MonoBehaviour
{

    #region Debug Settings
    [Header("Debug Settings")]
    [SerializeField] private bool _debug = false;
    #endregion

    [SerializeField] private GameObject _gameObjectToDestroy;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_debug) Debug.Log($"{collision.gameObject.name} entered the goal!");
        if (collision.gameObject == _gameObjectToDestroy)
        {
            Destroy(collision.gameObject);
        }
    }
}
