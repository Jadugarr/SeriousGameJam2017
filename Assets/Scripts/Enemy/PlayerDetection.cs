using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private EventManager eventManager = EventManager.Instance;
    [SerializeField] private GameObject enemy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            eventManager.FireEvent(EventTypes.PlayerSpotted, new PlayerSpottedEvent(enemy));
        }
    }
}