using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterConfig charConfig;
    [SerializeField] private PolygonCollider2D swordCollider;

    public Image progressHelm;

    //Private Stuff
    private EventManager eventManager = EventManager.Instance;

    private float gravity;
    private float jumpVelocity;
    private Vector2 velocity;
    private float velocityXSmoothing;
    private Vector2 input;
    private Animator playerAnimator;

    private bool knockback = false;
    private bool isAttacking = false;
    private int playerDir = 0;
    private bool doJump = false;
    private bool ableToAddJump = false;
    private bool ableToJump = true;
    private float jumpAcc = 0f;


    //PLAYERGAMELOGIC
    private int currentProgress = 5;

    private int currentProgStep = 0;
    private float currentSpeed = 0;
    private float progLossAcc = 0f;

    MovementController controller;

    void Start()
    {
        eventManager.RegisterForEvent(EventTypes.JumpEvent, OnJump);
        eventManager.RegisterForEvent(EventTypes.AxisInputEvent, OnAxisInput);
        eventManager.RegisterForEvent(EventTypes.AttackInput, OnAttackInput);
        eventManager.RegisterForEvent(EventTypes.PlayerHit, OnPlayerHit);
        eventManager.RegisterForEvent(EventTypes.PlayerRespawned, FellDown);
        controller = GetComponent<MovementController>();
        gravity = -(2 * charConfig.JumpHeight) / Mathf.Pow(charConfig.TimeToJumpMax, 2);
        jumpVelocity = Mathf.Abs(gravity) * charConfig.TimeToJumpMax;
        playerAnimator = GetComponent<Animator>();
        currentSpeed = charConfig.MovementSpeed0;
    }

    void OnDestroy()
    {
        eventManager.RemoveFromEvent(EventTypes.JumpEvent, OnJump);
        eventManager.RemoveFromEvent(EventTypes.AxisInputEvent, OnAxisInput);
        eventManager.RemoveFromEvent(EventTypes.AttackInput, OnAttackInput);
        eventManager.RemoveFromEvent(EventTypes.PlayerHit, OnPlayerHit);
    }

    private void UpdateProgressBar()
    {
        currentProgress = Mathf.Clamp(currentProgress, 0, 101);
        if (currentProgress >= 100)
        {
            if (currentProgStep != 5)
            {
                eventManager.FireEvent(EventTypes.ProgStepChangeEvent, new ProgStepChangeEvent(5));
                currentProgStep = 5;
            }
            currentSpeed = charConfig.MovementSpeed100;
        }
        else if (currentProgress >= 90)
        {
            if (currentProgStep != 4)
            {
                eventManager.FireEvent(EventTypes.ProgStepChangeEvent, new ProgStepChangeEvent(4));
                currentProgStep = 4;
            }
            currentSpeed = charConfig.MovementSpeed90;
        }
        else if (currentProgress >= 60)
        {
            if (currentProgStep != 4)
            {
                eventManager.FireEvent(EventTypes.ProgStepChangeEvent, new ProgStepChangeEvent(4));
                currentProgStep = 4;
            }
            currentSpeed = charConfig.MovementSpeed60;
        }
        else if (currentProgress >= 30)
        {
            if (currentProgStep != 2)
            {
                eventManager.FireEvent(EventTypes.ProgStepChangeEvent, new ProgStepChangeEvent(2));
                currentProgStep = 2;
            }
            currentSpeed = charConfig.MovementSpeed30;
        }
        else if (currentProgress >= 10)
        {
            if (currentProgStep != 1)
            {
                eventManager.FireEvent(EventTypes.ProgStepChangeEvent, new ProgStepChangeEvent(1));
                currentProgStep = 1;
            }
            currentSpeed = charConfig.MovementSpeed10;
        }
        else if (currentProgress < 10)
        {
            if (currentProgStep != 0)
            {
                eventManager.FireEvent(EventTypes.ProgStepChangeEvent, new ProgStepChangeEvent(0));
                currentProgStep = 0;
            }
            currentSpeed = charConfig.MovementSpeed0;
        }
        progressHelm.rectTransform.localPosition = new Vector3(-280f + ((560f / 100f) * currentProgress),
            progressHelm.rectTransform.localPosition.y, progressHelm.rectTransform.localPosition.z);
    }

    public Vector2 GetPlayerVel()
    {
        return velocity;
    }

    public MovementController GetMovController()
    {
        return controller;
    }

    public void PowerPillTaken()
    {
        eventManager.FireEvent(EventTypes.PillTaken, new PowerPillTakenEvent());
        currentProgress += charConfig.progressPerPill;
        UpdateProgressBar();
    }

    private void EnemySlain()
    {
        eventManager.FireEvent(EventTypes.EnemyHit, new EnemyHitEvent());
        currentProgress += charConfig.progressPerEnemySlain;
        UpdateProgressBar();
    }

    public void FellDown(IEvent respawnedEvent)
    {
        currentProgress -= charConfig.progressLossPerFall;
        UpdateProgressBar();
    }

    private void OnPlayerHit(IEvent evt)
    {
        currentProgress -= charConfig.progressLossPerFall;
        UpdateProgressBar();
        velocity.x = charConfig.KnockbackStrength.x;
        knockback = true;
    }

    private void OnAxisInput(IEvent axisInputEvent)
    {
        input = ((AxisInputEvent) axisInputEvent).input;
    }

    private void OnJump(IEvent jumpEvent)
    {
        JumpInputType inputType = ((JumpEvent) jumpEvent).jumpInputType;

        if (controller.collisions.below && ableToJump && inputType == JumpInputType.jumpDown)
        {
            ableToJump = false;
            ableToAddJump = true;
            doJump = true;
        }
        if (ableToAddJump && inputType == JumpInputType.jumpHold)
        {
            if (jumpAcc < charConfig.jumpTime)
            {
                jumpAcc += Time.deltaTime;
                doJump = true;
            }
            else
            {
                jumpAcc = 0f;
                ableToAddJump = false;
            }
        }
        if (!ableToJump && inputType == JumpInputType.jumpRelease)
        {
            ableToJump = true;
            ableToAddJump = false;
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
        if (velocity.x < 0.1f)
        {
            progLossAcc += Time.deltaTime;
            if (progLossAcc > charConfig.lossPerTimeSpeed)
            {
                currentProgress -= 1;
                progLossAcc = 0f;
                UpdateProgressBar();
            }
        }

        currentProgress = Mathf.Clamp(currentProgress, 0, 101);

        Mathf.Clamp(currentProgress, 0, 100);
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0f;
        }

        if (doJump)
        {
            velocity.y = jumpVelocity;
        }

        if (knockback)
        {
            //       velocity.y = charConfig.KnockbackStrength.y;
        }

        float targetVelocityX = input.x * currentSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? charConfig.AccelerationTimeGrounded : charConfig.AccelerationTimeAir);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime);
        doJump = false;
        knockback = false;
        input = Vector2.zero;
    }

    public void Attack()
    {
        eventManager.FireEvent(EventTypes.PlayerAttack, new PlayerAttackEvent());
        Collider2D[] results = new Collider2D[5];
        if (swordCollider.OverlapCollider(new ContactFilter2D().NoFilter(), results) > 0)
        {
            foreach (Collider2D collider in results)
            {
                if (collider != null && collider.tag == "Enemy")
                {
                    Destroy(collider.gameObject);
                    EnemySlain();
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