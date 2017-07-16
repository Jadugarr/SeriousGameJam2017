using System;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class EnemyBehaviour : MonoBehaviour
{
    [HideInInspector] public EnemyConfigObject EnemyConfig;

    [SerializeField] private PolygonCollider2D attackArea;
    private MovementController controller;
    private Vector2 velocity;
    private bool startJump = false;
    private int direction = 1;
    private float velocityXSmoothing;
    private Animator enemyAnimator;

    private EventManager eventManager = EventManager.Instance;

    private bool isAttacking = false;

    void Awake()
    {
        controller = GetComponent<MovementController>();
        enemyAnimator = GetComponent<Animator>();
        eventManager.RegisterForEvent(EventTypes.EnemyDirectionTriggered, OnDirectionTrigger);
        eventManager.RegisterForEvent(EventTypes.PlayerSpotted, OnPlayerSpotted);
    }

    void OnDestroy()
    {
        eventManager.RemoveFromEvent(EventTypes.EnemyDirectionTriggered, OnDirectionTrigger);
        eventManager.RemoveFromEvent(EventTypes.PlayerSpotted, OnPlayerSpotted);
    }

    void Update()
    {
        if (isAttacking == false)
        {
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0f;
            }

            if (startJump)
            {
                velocity.y = EnemyConfig.JumpVelocity;
            }

            velocity.x = direction * EnemyConfig.MovementSpeed;
            velocity.y += EnemyConfig.Gravity * Time.deltaTime;
            controller.Move(new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime);
            startJump = false;
        }
    }

    private void OnDirectionTrigger(IEvent evt)
    {
        EnemyDirectionTriggeredEvent evtArgs = (EnemyDirectionTriggeredEvent) evt;

        if (evtArgs.Enemy == gameObject)
        {
            direction *= -1;
            gameObject.transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    private void OnPlayerSpotted(IEvent evt)
    {
        PlayerSpottedEvent evtArgs = (PlayerSpottedEvent) evt;

        if (isAttacking == false && evtArgs.enemyGameObject == gameObject)
        {
            isAttacking = true;
            enemyAnimator.SetBool("Attacking", true);
        }
    }

    public void AttackPlayer()
    {
        Collider2D[] results = new Collider2D[5];
        if (attackArea.OverlapCollider(new ContactFilter2D().NoFilter(), results) > 0)
        {
            foreach (Collider2D collider in results)
            {
                if (collider != null && collider.tag == "Player")
                {
                    eventManager.FireEvent(EventTypes.PlayerHit, new PlayerHitEvent());
                }
            }
        }
    }

    public void AttackFinished()
    {
        isAttacking = false;
        enemyAnimator.SetBool("Attacking", false);
    }
}