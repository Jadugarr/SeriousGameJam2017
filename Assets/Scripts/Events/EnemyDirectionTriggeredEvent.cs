using UnityEngine;

public class EnemyDirectionTriggeredEvent : IEvent
{
    public GameObject Enemy;

    public EnemyDirectionTriggeredEvent(GameObject enemy)
    {
        Enemy = enemy;
    }
}
