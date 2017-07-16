using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    private GameObject playerGameObject;
    private EventManager eventManager = EventManager.Instance;

    void Awake()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerGameObject.transform.position.x >= gameObject.transform.position.x)
        {
            eventManager.FireEvent(EventTypes.PlayerPassedCheckpoint, new PlayerPassedCheckpointEvent(gameObject));
            gameObject.SetActive(false);
        }
    }
}