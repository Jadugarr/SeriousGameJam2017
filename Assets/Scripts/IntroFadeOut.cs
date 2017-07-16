using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class IntroFadeOut : MonoBehaviour {
	
	[SerializeField]
	protected Transform player;

	[SerializeField]
	protected Transform camera;

	[SerializeField]
	protected CanvasGroup fade;

	[SerializeField]
	protected CanvasGroup title;

	[SerializeField]
	protected float fadeDuration;

	[SerializeField]
	protected float fallingDuration;

	[SerializeField]
	protected float fallingDistance;

	[SerializeField]
	protected float timeUntilThumpSound;

	[SerializeField]
	protected float timeUntilSceneTransition;

	[SerializeField]
	protected Button exitButton;

	[SerializeField]
	protected Button startButton;

	[SerializeField]
	protected AudioSource windSound;

	[SerializeField]
	protected AudioSource thumpSound;

	protected float endTimeStamp = -1f;
	protected float fallingTimeStamp = -1;
	protected float fadeStartTimeStamp = -1;
	protected float playerStartY = -1;
	protected bool thumpSoundPlayed = false;

	// Use this for initialization
	void Start () 
	{
		playerStartY = player.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(fallingTimeStamp != -1)
		{
			fallingProgress();
		}
		if(fadeStartTimeStamp != -1)
		{
			fadeProgress();
		}
		if(endTimeStamp != -1)
		{
			endProgress();
		}
	}

	public void startSequence()
	{
		startButton.gameObject.SetActive(false);
		exitButton.gameObject.SetActive(false);
		startFalling();
	}

	protected void startFalling()
	{
		fallingTimeStamp = Time.realtimeSinceStartup;
	}

	protected void fallingProgress()
	{
		float progress = Mathf.Clamp((Time.realtimeSinceStartup - fallingTimeStamp) / fallingDuration, 0f, 1f);
		if(progress == 1f)
		{
			fallingTimeStamp = -1f;
			title.alpha = 0f;
			startFade();
		}
		else
		{
			title.alpha = 1f - progress;
			Vector3 pos = player.position;
			pos.y = playerStartY - fallingDistance * progress;
			player.position = pos;
		}
	}

	protected void startFade()
	{
		fadeStartTimeStamp = Time.realtimeSinceStartup;
		fade.gameObject.SetActive(true);
		fade.alpha = 0f;
	}

	protected void endFade()
	{
		endTimeStamp = Time.realtimeSinceStartup;
		fadeStartTimeStamp = -1f;
		fade.alpha = 1f;
		windSound.Stop();
	}

	protected void endProgress()
	{
		float passedSecs = Time.realtimeSinceStartup - endTimeStamp;
		if(passedSecs >= timeUntilSceneTransition)
		{
			SceneManager.LoadScene(1);
		}
		else if( !thumpSoundPlayed && passedSecs >= timeUntilThumpSound)
		{
			thumpSoundPlayed = true;
			thumpSound.Play();
		}
	}

	protected void fadeProgress()
	{
		float progress = Mathf.Clamp((Time.realtimeSinceStartup - fadeStartTimeStamp) / fadeDuration, 0f, 1f);
		if(progress == 1f)
		{
			endFade();
		}
		else
		{
			if(progress > 0.05f)
			{
				Vector3 pos = camera.position;
				pos.y -= 0.035f + progress / 100f;
				camera.position = pos;
			}

			fade.alpha = progress;
			windSound.volume = 1f - progress;
		}
	}
}
