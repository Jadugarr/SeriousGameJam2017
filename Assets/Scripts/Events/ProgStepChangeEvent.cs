using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgStepChangeEvent : IEvent {

    public int progStep;

    public ProgStepChangeEvent(int progStep)
    {
        this.progStep = progStep;
    }
	
}
