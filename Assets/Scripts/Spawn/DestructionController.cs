using UnityEngine;

public class DestructionController : MonoBehaviour
{
    private EventManager eventManager = EventManager.Instance;

    public void OnTriggerEnter2D(Collider2D other)
    {
        eventManager.FireEvent(EventTypes.EnteredDestructionZone, new EnteredDestructionZoneEvent(other.gameObject));
    }
}