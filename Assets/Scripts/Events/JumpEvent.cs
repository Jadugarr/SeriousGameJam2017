using UnityEngine;

public enum JumpInputType
{
    jumpDown,
    jumpHold,
    jumpRelease
}

public class JumpEvent : IEvent
{
    public JumpInputType jumpInputType;

    public JumpEvent(JumpInputType inputType)
    {
        jumpInputType = inputType;
    }


}
