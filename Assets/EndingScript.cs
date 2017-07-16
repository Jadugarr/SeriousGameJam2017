using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScript : MonoBehaviour {
	[SerializeField]
	protected Animator player;

	[SerializeField]
	protected CanvasGroup fadeGroup;

	[SerializeField]
	protected float startTime;

	[SerializeField]
	protected float playerTargetX;

	[SerializeField]
	protected float walkSpeed;

	[SerializeField]
	protected float cameraTargetY;

	[SerializeField]
	protected float cameraSpeed;

	[SerializeField]
	protected Transform cameraTransform;

	[SerializeField]
	protected float fadeOutTime;

	[SerializeField]
	protected AudioSource clangSound;

	private int phase = 0;
	float startTimestamp;
	float endTimestamp;

	public void PlayClangSound()
	{
		clangSound.pitch = Random.Range(0.5f, 1.5f);
		clangSound.Play();
	}

	// Use this for initialization
	void Awake () {
		startTimestamp = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float timePassed =  Time.realtimeSinceStartup - startTimestamp;

		if(phase == 0 && timePassed >= startTime)
		{
			phase = 1;
			startWalking();
		}
		else if( phase == 1)
		{
			walking();
		}
		else if( phase == 3)
		{
			cameraMove();
		}
		else if( phase == 4 && (
			Input.GetAxis("Fire1") > 0f 
			|| Input.GetAxis("Fire2") > 0f
			|| Input.GetAxis("Jump") > 0f))
		{
			phase = 5;
		}
		else if( phase == 5)
		{
			fadeOut();
		}
	}

	protected void startWalking()
	{
		player.SetTrigger("walk");
	}

	protected void walking()
	{
		Vector3 pos = player.transform.position;
		if(pos.x < playerTargetX)
		{
			pos.x += walkSpeed;
			player.transform.position = pos;
		}
		else
		{
			startSexyTime();
		}
	}

	protected void startSexyTime()
	{
		phase = 2;
		player.SetTrigger("strip");
	}

	protected void finishSexyTime()
	{
		phase = 3;
	}

	protected void cameraMove()
	{
		Vector3 pos = cameraTransform.position;
		if(pos.y < cameraTargetY)
		{
			pos.y += cameraSpeed;
			cameraTransform.position = pos;
		}
		else
		{
			theEnd();
		}
	}

	protected void theEnd()
	{
		phase = 4;
		endTimestamp = Time.realtimeSinceStartup;
	}

	protected void fadeOut()
	{
		float progress = Mathf.Clamp((Time.realtimeSinceStartup - endTimestamp) / fadeOutTime, 0f, 1f);

		if( progress == 1.0f )
		{
			fadeGroup.alpha = 1f;

			phase = 5;
			SceneManager.LoadScene( 0);
		}
		else
		{
			fadeGroup.alpha = progress;
		}
	}
}
