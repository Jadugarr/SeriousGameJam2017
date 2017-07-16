using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	private const string ANIMATOR_HORIZONTAL_SPEED = "horizontal_speed";
	private const string ANIMATOR_VERTICAL_SPEED = "vertical_speed";
	private const string ANIMATOR_JUMP = "jump";
	private const string ANIMATOR_ATTACK = "attack";
	private const string ANIMATOR_STRIP = "stripping";

	private EventManager eventManager = EventManager.Instance;
	private bool isLookingLeft = false;

	[SerializeField]
	protected Animator feetAnimator;

	[SerializeField]
	protected Animator bodyAnimator;

	[SerializeField]
	protected PlayerController playerController;



	void Start()
	{
		eventManager.RegisterForEvent(EventTypes.JumpEvent, OnJump);
		eventManager.RegisterForEvent(EventTypes.AttackInput, OnAttackInput);
		eventManager.RegisterForEvent(EventTypes.EndSequenceEvent, OnPlayerStrip);
	}

	void OnDestroy()
	{
		eventManager.RemoveFromEvent(EventTypes.JumpEvent, OnJump);
		eventManager.RemoveFromEvent(EventTypes.AttackInput, OnAttackInput);
		eventManager.RemoveFromEvent(EventTypes.EndSequenceEvent, OnPlayerStrip);
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector2 velocity = playerController.GetPlayerVel();

		if( isLookingLeft && velocity.x > 0.2)
		{
			// turn right
			Vector3 bodyScale = bodyAnimator.gameObject.transform.localScale;
			Vector3 feetScale = feetAnimator.gameObject.transform.localScale;
			bodyScale.x *= -1f;
			feetScale.x *= -1f;
			bodyAnimator.gameObject.transform.localScale = bodyScale;
			feetAnimator.gameObject.transform.localScale = feetScale;

			isLookingLeft = false;
		}
		else if( !isLookingLeft && velocity.x < -0.2)
		{
			// turn left
			Vector3 bodyScale = bodyAnimator.gameObject.transform.localScale;
			Vector3 feetScale = feetAnimator.gameObject.transform.localScale;
			bodyScale.x *= -1f;
			feetScale.x *= -1f;
			bodyAnimator.gameObject.transform.localScale = bodyScale;
			feetAnimator.gameObject.transform.localScale = feetScale;

			isLookingLeft = true;
		}
		float runSpeed = Mathf.Abs(velocity.x);
		float verticalSpeed = velocity.y + 1.5f;

		feetAnimator.SetFloat(ANIMATOR_HORIZONTAL_SPEED, runSpeed);
		feetAnimator.SetFloat(ANIMATOR_VERTICAL_SPEED, verticalSpeed);

		bodyAnimator.SetFloat(ANIMATOR_HORIZONTAL_SPEED, runSpeed);
		bodyAnimator.SetFloat(ANIMATOR_VERTICAL_SPEED, verticalSpeed);
	}

	private void OnJump(IEvent jumpEvent)
	{
		feetAnimator.SetTrigger(ANIMATOR_JUMP);
		bodyAnimator.SetTrigger(ANIMATOR_JUMP);
	}

	private void OnAttackInput(IEvent args)
	{
		bodyAnimator.SetTrigger(ANIMATOR_ATTACK);
	}

	private void OnPlayerStrip(IEvent args)
	{
		feetAnimator.SetBool(ANIMATOR_STRIP, true);
		bodyAnimator.SetBool(ANIMATOR_STRIP, true);
	}
}
