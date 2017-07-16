using UnityEngine;

public class PlatformComponent : MonoBehaviour
{
    public PlatformType PlatformType;
    public int platformWidth;

    private GameObject destructionZone;
    private CheckpointBehaviour[] checkpoints;
    private EventManager eventManager = EventManager.Instance;

    void Awake()
    {
        destructionZone = GameObject.FindGameObjectWithTag("DestructionMarker");
        checkpoints = GetComponentsInChildren<CheckpointBehaviour>();
    }

    void Update()
    {
        if (gameObject.transform.position.x <= destructionZone.transform.position.x)
        {
            eventManager.FireEvent(EventTypes.EnteredDestructionZone, new EnteredDestructionZoneEvent(gameObject));
        }
    }

    public void Activate()
    {
        foreach (CheckpointBehaviour checkpointBehaviour in checkpoints)
        {
            checkpointBehaviour.gameObject.SetActive(true);
        }
    }
}