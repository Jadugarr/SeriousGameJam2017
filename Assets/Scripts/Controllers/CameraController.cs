using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private CameraConfig camConfig;

    //Public Stuff
    public Transform playerTransform;
    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        //Check if Out of Bounds
        if(playerTransform.position.x > (this.transform.position.x + camConfig.horizontalDelta))
        {
            float deltaX = playerTransform.position.x - (this.transform.position.x + camConfig.horizontalDelta) ;
            this.transform.Translate(deltaX,0f,0f);
        }
        if((playerTransform.position.x - this.transform.position.x) > camConfig.dampFactor)
        {
            this.transform.Translate(Time.deltaTime * camConfig.camSlowSpeed,0f,0f);
        }
        if (playerTransform.position.y > (this.transform.position.y + camConfig.verticalDelta))
        {
            float deltaY = playerTransform.position.y - (this.transform.position.y + camConfig.horizontalDelta);
            this.transform.Translate(0f, deltaY, 0f);
        }
        if ((playerTransform.position.y - this.transform.position.y) > camConfig.dampFactor)
        {
            this.transform.Translate(0f,Time.deltaTime * camConfig.camSlowSpeed, 0f);
        }
        if (playerTransform.position.y < (this.transform.position.y - camConfig.verticalDelta))
        {
            float deltaY = playerTransform.position.y - (this.transform.position.y - camConfig.horizontalDelta);
            this.transform.Translate(0f, deltaY, 0f);
        }
        /*if ((playerTransform.position.y + this.transform.position.y) > camConfig.dampFactor)
        {
            this.transform.Translate(0f, - Time.deltaTime * camConfig.camSlowSpeed, 0f);
        }
        */
    }
}
