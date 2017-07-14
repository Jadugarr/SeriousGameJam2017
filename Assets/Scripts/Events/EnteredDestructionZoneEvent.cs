using UnityEngine;

public class EnteredDestructionZoneEvent : IEvent
{
    public GameObject ObjectToDestroy;

    public EnteredDestructionZoneEvent(GameObject objectToDestroy)
    {
        ObjectToDestroy = objectToDestroy;
    }
}