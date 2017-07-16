using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInScript : MonoBehaviour {

	protected CanvasGroup fadeGroup;

	[SerializeField]
	protected float fadeDuration = 0.6f;

	[SerializeField]
	protected AudioSource fadeAudio = null;

	protected float fadeStartTimeStamp;
	protected float maxVol;

	// Use this for initialization
	void Awake () {
		fadeStartTimeStamp = Time.realtimeSinceStartup;
		fadeGroup = GetComponent<CanvasGroup>();
		if(fadeAudio != null)
		{
			maxVol = fadeAudio.volume;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float progress = Mathf.Clamp((Time.realtimeSinceStartup - fadeStartTimeStamp) / fadeDuration, 0f, 1f);
		if(progress == 1f)
		{
			Destroy(gameObject);
		}
		else
		{
			fadeGroup.alpha = 1f - progress;
			if(fadeAudio != null)
			{
				fadeAudio.volume = maxVol * progress;
			}
		}
	}
}
