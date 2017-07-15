using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private EventManager eventManager = EventManager.Instance;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Jump") > 0.1f)
        {
            eventManager.FireEvent(EventTypes.JumpEvent, new JumpEvent());
        }
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            eventManager.FireEvent(EventTypes.AxisInputEvent, new AxisInputEvent(input));
        }
        if (Input.GetAxisRaw("Fire1") > 0f)
        {
            eventManager.FireEvent(EventTypes.AttackInput, new AttackInputEvent());
        }
    }
}