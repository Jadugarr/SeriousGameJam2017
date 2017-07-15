using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionEvent : IEvent {

    public int playerDirection;

    public PlayerDirectionEvent(int dir)
    {
        this.playerDirection = dir;
    }

}
