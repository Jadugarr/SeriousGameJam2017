using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private GameObject respawnMarker;

    private GameObject currentCheckpoint;
    private EventManager eventManager = EventManager.Instance;

    void Awake()
    {
        eventManager.RegisterForEvent(EventTypes.PlayerPassedCheckpoint, OnCheckpointReached);
    }

    void OnDestroy()
    {
        eventManager.RemoveFromEvent(EventTypes.PlayerPassedCheckpoint, OnCheckpointReached);
    }

    private void OnCheckpointReached(IEvent evt)
    {
        PlayerPassedCheckpointEvent evtArgs = (PlayerPassedCheckpointEvent) evt;

        currentCheckpoint = evtArgs.Checkpoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerGameObject.transform.position.y <= respawnMarker.transform.position.y)
        {
            playerGameObject.transform.position = currentCheckpoint.transform.position;
            eventManager.FireEvent(EventTypes.PlayerRespawned, new PlayerRespawnedEvent());
        }
    }
}