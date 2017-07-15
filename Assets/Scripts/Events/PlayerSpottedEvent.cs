using UnityEngine;

public class PlayerSpottedEvent : IEvent
{
    public GameObject enemyGameObject;

    public PlayerSpottedEvent(GameObject enemyGameObject)
    {
        this.enemyGameObject = enemyGameObject;
    }
}
