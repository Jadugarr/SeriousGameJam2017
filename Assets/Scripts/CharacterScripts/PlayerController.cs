using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterConfig charConfig;
    [SerializeField] private PolygonCollider2D swordCollider;


    //Private Stuff
    private EventManager eventManager = EventManager.Instance;

    private float gravity;
    private float jumpVelocity;
    private Vector2 velocity;
    private float velocityXSmoothing;
    private Vector2 input;
    private Animator playerAnimator;

    private bool knockback = false;
    private bool startJump = false;
    private bool isAttacking = false;
    private int playerDir = 0;

    MovementController controller;

    void Start()
    {
        eventManager.RegisterForEvent(EventTypes.JumpEvent, OnJump);
        eventManager.RegisterForEvent(EventTypes.AxisInputEvent, OnAxisInput);
        eventManager.RegisterForEvent(EventTypes.AttackInput, OnAttackInput);
        eventManager.RegisterForEvent(EventTypes.PlayerHit, OnPlayerHit);
        controller = GetComponent<MovementController>();
        gravity = -(2 * charConfig.JumpHeight) / Mathf.Pow(charConfig.TimeToJumpMax, 2);
        jumpVelocity = Mathf.Abs(gravity) * charConfig.TimeToJumpMax;
        playerAnimator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
	eventManager.RemoveFromEvent(EventTypes.JumpEvent, OnJump);
        eventManager.RemoveFromEvent(EventTypes.AxisInputEvent, OnAxisInput);
        eventManager.RemoveFromEvent(EventTypes.AttackInput, OnAttackInput);
        eventManager.RemoveFromEvent(EventTypes.PlayerHit, OnPlayerHit);
        eventManager.RemoveFromEvent(EventTypes.PlayerHit, OnPlayerHit);
    }
    
    public Vector2 GetPlayerVel()
    {
        return velocity;
    }

    private void OnPlayerHit(IEvent evt)
    {
        velocity.x = charConfig.KnockbackStrength.x;
        knockback = true;
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

    private void OnAttackInput(IEvent args)
    {
        if (isAttacking == false)
        {
            playerAnimator.SetBool("Attacking", true);
            isAttacking = true;
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

        if (knockback)
        {
            velocity.y = charConfig.KnockbackStrength.y;
        }

        float targetVelocityX = input.x * charConfig.MovementSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? charConfig.AccelerationTimeGrounded : charConfig.AccelerationTimeAir);
        velocity.y += gravity * Time.deltaTime;
        //
        /*if(playerDir != 1 && velocity.x > 0f)
        {
            playerDir = 1;
            eventManager.FireEvent(EventTypes.PlayerDirectionEvent, new PlayerDirectionEvent(1));
        } 
        if(playerDir != 0 && velocity.x == 0f)
        {
            playerDir = 0;
            eventManager.FireEvent(EventTypes.PlayerDirectionEvent, new PlayerDirectionEvent(0));
        } 
        if(playerDir != -1 && velocity.x < 0f)
        {
            playerDir = -1;
            eventManager.FireEvent(EventTypes.PlayerDirectionEvent, new PlayerDirectionEvent(-1));
        }*/
        //
        controller.Move(new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime);
        startJump = false;
        knockback = false;
        input = Vector2.zero;
    }

    public void Attack()
    {
        Collider2D[] results = new Collider2D[5];
        if (swordCollider.OverlapCollider(new ContactFilter2D().NoFilter(), results) > 0)
        {
            foreach (Collider2D collider in results)
            {
                if (collider != null && collider.tag == "Enemy")
                {
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    public void AttackFinished()
    {
        playerAnimator.SetBool("Attacking", false);
        isAttacking = false;
    }
}