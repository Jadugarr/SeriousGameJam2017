using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill_SpawnBlock : MonoBehaviour {

    public GameObject hiddenBlock;
    //Private Stuff
    private SpriteRenderer[] hiddenBlockSR;
    private float lerpAcc = 0f;
    private Color startColor;

	// Use this for initialization
	void Start () {
        hiddenBlockSR = hiddenBlock.GetComponentsInChildren<SpriteRenderer>();
        startColor = new Color(1f, 1f, 1f, 0.01f);
        for(int i = 0; i < hiddenBlockSR.Length; i++)
        {
            hiddenBlockSR[i].color = startColor;
        }
        hiddenBlock.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(hiddenBlock.activeSelf && hiddenBlockSR[0].color.a < 0.99f)
        {
            lerpAcc += Time.deltaTime;
            for (int i = 0; i < hiddenBlockSR.Length; i++)
            {
                hiddenBlockSR[i].color = Color.Lerp(startColor, new Color(1f, 1f, 1f, 1f), lerpAcc);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hiddenBlock.SetActive(true);
        }
    }
}
