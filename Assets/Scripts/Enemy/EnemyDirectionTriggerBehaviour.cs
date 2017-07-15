using UnityEngine;

public class EnemyDirectionTriggerBehaviour : MonoBehaviour
{
    private EventManager eventManager = EventManager.Instance;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            eventManager.FireEvent(EventTypes.EnemyDirectionTriggered,
                new EnemyDirectionTriggeredEvent(other.gameObject));
        }
    }
}