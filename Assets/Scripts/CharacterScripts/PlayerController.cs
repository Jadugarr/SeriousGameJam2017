using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float jumpHeight = 4f;
    public float timeToJumpMax = .4f;

    public float accelerationTimeAir = .4f;
    public float accelerationTimeGrounded = .25f;
    public float moveSpeed = 6f;

    //Private Stuff
    private float gravity;
    private float jumpVelocity;
    private Vector2 velocity;
    private float velocityXSmoothing;

    MovementController controller;

	void Start () {
        controller = GetComponent<MovementController>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpMax, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpMax;
    }
	
	void Update () {

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0f;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            input.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAir);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime);
	}
}
