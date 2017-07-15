using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill_SpawnBlock : MonoBehaviour {

    public GameObject hiddenBlock;
    //Private Stuff
    private SpriteRenderer hiddenBlockSR;
    private float lerpAcc = 0f;
    private Color startColor;

	// Use this for initialization
	void Start () {
        hiddenBlockSR = hiddenBlock.GetComponent<SpriteRenderer>();
        startColor = new Color(hiddenBlockSR.color.r, hiddenBlockSR.color.g, hiddenBlockSR.color.b, hiddenBlockSR.color.a);
        hiddenBlock.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(hiddenBlock.activeSelf && hiddenBlockSR.color.a < 0.99f)
        {
            lerpAcc += Time.deltaTime;
            hiddenBlockSR.color = Color.Lerp(startColor, new Color(1f, 1f, 1f, 1f), lerpAcc);
            Debug.Log("LERP");
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
