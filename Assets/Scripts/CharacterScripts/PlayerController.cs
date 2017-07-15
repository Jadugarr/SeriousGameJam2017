using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterConfig charConfig;


    //Private Stuff
    private EventManager eventManager = EventManager.Instance;

    private float gravity;
    private float jumpVelocity;
    private Vector2 velocity;
    private float velocityXSmoothing;
    private Vector2 input;

    private bool startJump = false;

    MovementController controller;

    void Start()
    {
        eventManager.RegisterForEvent(EventTypes.JumpEvent, OnJump);
        eventManager.RegisterForEvent(EventTypes.AxisInputEvent, OnAxisInput);
        controller = GetComponent<MovementController>();
        gravity = -(2 * charConfig.JumpHeight) / Mathf.Pow(charConfig.TimeToJumpMax, 2);
        jumpVelocity = Mathf.Abs(gravity) * charConfig.TimeToJumpMax;
    }

    private void OnAxisInput(IEvent axisInputEvent)
    {
        input = ((AxisInputEvent) axisInputEvent).input;
    }

    private void OnJump(IEvent jumpEvent)
    {
        if (controller.collisions.below)
        {
            startJump = true;
        }
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0f;
        }

        if (startJump)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * charConfig.MovementSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? charConfig.AccelerationTimeGrounded : charConfig.AccelerationTimeAir);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime);
        startJump = false;
        input = Vector2.zero;
    }

    public void Attack()
    {
        Debug.Log("Attack!");
    }
}