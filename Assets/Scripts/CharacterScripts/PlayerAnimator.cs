using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	private const string ANIMATOR_HORIZONTAL_SPEED = "horizontal_speed";
	private const string ANIMATOR_VERTICAL_SPEED = "vertical_speed";
	private const string ANIMATOR_JUMP = "jump";
	private const string ANIMATOR_ATTACK = "attack";
	private const string ANIMATOR_STRIP = "stripping";
	private const string ANIMATOR_AIRBORNE = "airborne";

	private EventManager eventManager = EventManager.Instance;
	private bool isLookingLeft = false;
	private bool isAirborne = true;

	[SerializeField]
	protected Animator feetAnimator;

	[SerializeField]
	protected Animator bodyAnimator;

	[SerializeField]
	protected PlayerController playerController;



	void Start()
	{
		eventManager.RegisterForEvent(EventTypes.AttackInput, OnAttackInput);
		eventManager.RegisterForEvent(EventTypes.EndSequenceEvent, OnPlayerStrip);
	}

	void OnDestroy()
	{
		eventManager.RemoveFromEvent(EventTypes.AttackInput, OnAttackInput);
		eventManager.RemoveFromEvent(EventTypes.EndSequenceEvent, OnPlayerStrip);
	}

	// Update is called once per frame
	void Update () 
	{
		Vector2 velocity = playerController.GetPlayerVel();
		float runSpeed = Mathf.Abs(velocity.x);
		float verticalSpeed = velocity.y;

		isAirborne = !playerController.GetMovController().collisions.below;
		feetAnimator.SetBool(ANIMATOR_AIRBORNE, isAirborne);
		bodyAnimator.SetBool(ANIMATOR_AIRBORNE, isAirborne);

		if( isLookingLeft && velocity.x > 1)
		{
			// turn right
			Vector3 scale = playerController.gameObject.transform.localScale;
			scale.x *= -1f;
			playerController.gameObject.transform.localScale = scale;

			isLookingLeft = false;
		}
		else if( !isLookingLeft && velocity.x < -1)
		{
			// turn left
			Vector3 scale = gameObject.transform.localScale;
			scale.x *= -1f;
			gameObject.transform.localScale = scale;

			isLookingLeft = true;
		}

		feetAnimator.SetFloat(ANIMATOR_HORIZONTAL_SPEED, runSpeed);
		feetAnimator.SetFloat(ANIMATOR_VERTICAL_SPEED, verticalSpeed);

		bodyAnimator.SetFloat(ANIMATOR_HORIZONTAL_SPEED, runSpeed);
		bodyAnimator.SetFloat(ANIMATOR_VERTICAL_SPEED, verticalSpeed);
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
