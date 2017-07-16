using UnityEngine;

public class PlayerPassedCheckpointEvent : IEvent
{
    public GameObject Checkpoint;

    public PlayerPassedCheckpointEvent(GameObject checkpoint)
    {
        Checkpoint = checkpoint;
    }
}
