using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyBehaviour : MonoBehaviour
{
    [HideInInspector]
    public EnemyConfigObject EnemyConfig;

    private MovementController controller;
    private SpriteRenderer spriteRenderer;
    private Vector2 velocity;
    private bool startJump = false;
    private int direction = 1;
    private float velocityXSmoothing;

    private EventManager eventManager = EventManager.Instance;

    void Awake()
    {
        controller = GetComponent<MovementController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        eventManager.RegisterForEvent(EventTypes.EnemyDirectionTriggered, OnDirectionTrigger);
    }

    void OnDestroy()
    {
        eventManager.RemoveFromEvent(EventTypes.EnemyDirectionTriggered, OnDirectionTrigger);
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0f;
        }

        if (startJump)
        {
            velocity.y = EnemyConfig.JumpVelocity;
        }

        float targetVelocityX = direction * EnemyConfig.MovementSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? EnemyConfig.AccelerationTimeGrounded : EnemyConfig.AccelerationTimeAir);
        velocity.y += EnemyConfig.Gravity * Time.deltaTime;
        controller.Move(new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime);
        startJump = false;
    }

    private void OnDirectionTrigger(IEvent evt)
    {
        EnemyDirectionTriggeredEvent evtArgs = (EnemyDirectionTriggeredEvent) evt;

        if (evtArgs.Enemy == gameObject)
        {
            direction *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}