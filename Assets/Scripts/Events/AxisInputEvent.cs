using UnityEngine;

public class AxisInputEvent : IEvent {

    public Vector2 input;

	public AxisInputEvent(Vector2 input)
    {
        this.input = input;
    }
}
