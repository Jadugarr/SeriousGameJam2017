using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour {

    [SerializeField] private ParallaxConfig paraConfig;

    public Transform[] frontParaArr;
    public Transform[] midParaArr;
    public Transform[] backParaArr;

    private EventManager eventManager = EventManager.Instance;
    private int playerDir;
    private GameObject cam;

    // Use this for initialization
    void Start () {
        //eventManager.RegisterForEvent(EventTypes.AxisInputEvent, PlayerDirEvent);
        cam = GameObject.FindGameObjectWithTag("Camera");
    }

    private void PlayerDirEvent(IEvent playerDirEvent)
    {
        playerDir = ((PlayerDirectionEvent)playerDirEvent).playerDirection;
    }

	
	// Update is called once per frame
	void Update () {
        /*
        if (playerDir == 0)
        {
            //Nothing moves
        } else if(playerDir == 1)
        {
            for(int i = 0; i < frontParaArr.Length; i++)
            {
                frontParaArr[i].Translate(paraConfig.Front_ParallaxSpeed * Time.deltaTime , 0f, 0f);
            }
            for (int i = 0; i < midParaArr.Length; i++)
            {
                midParaArr[i].Translate(paraConfig.Front_ParallaxSpeed * Time.deltaTime, 0f, 0f);
            }
            for (int i = 0; i < backParaArr.Length; i++)
            {
                backParaArr[i].Translate(paraConfig.Front_ParallaxSpeed * Time.deltaTime, 0f, 0f);
            }
        } else if(playerDir == -1)
        {
            for (int i = 0; i < frontParaArr.Length; i++)
            {
                frontParaArr[i].Translate(-paraConfig.Front_ParallaxSpeed * Time.deltaTime, 0f, 0f);
            }
            for (int i = 0; i < midParaArr.Length; i++)
            {
                midParaArr[i].Translate(-paraConfig.Front_ParallaxSpeed * Time.deltaTime, 0f, 0f);
            }
            for (int i = 0; i < backParaArr.Length; i++)
            {
                backParaArr[i].Translate(-paraConfig.Front_ParallaxSpeed * Time.deltaTime, 0f, 0f);
            }
        }
        */
        Vector2 currVel = cam.GetComponent<CameraController>().camVelocity;
        //Debug.Log(currVel);
        for (int i = 0; i < frontParaArr.Length; i++)
        {
            frontParaArr[i].Translate(-currVel.x * paraConfig.Front_ParallaxSpeed * Time.deltaTime, 0f, 0f);
            if(frontParaArr[i].transform.localPosition.x > paraConfig.boundaryRight)
            {
                frontParaArr[i].transform.Translate(-80f, 0f, 0f);
            }
            if (frontParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
            {
                frontParaArr[i].transform.Translate(80f, 0f, 0f);
            }
        }
        for (int i = 0; i < midParaArr.Length; i++)
        {
            midParaArr[i].Translate(-currVel.x * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
            if (midParaArr[i].transform.localPosition.x > paraConfig.boundaryRight)
            {
                midParaArr[i].transform.Translate(-80f, 0f, 0f);
            }
            if (midParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
            {
                midParaArr[i].transform.Translate(80f, 0f, 0f);
            }
        }
        for (int i = 0; i < backParaArr.Length; i++)
        {
            backParaArr[i].Translate(-currVel.x * paraConfig.Back_ParallaxSpeed * Time.deltaTime, 0f, 0f);
            if (backParaArr[i].transform.localPosition.x > paraConfig.boundaryRight)
            {
                backParaArr[i].transform.Translate(-80f, 0f, 0f);
            }
            if (backParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
            {
                backParaArr[i].transform.Translate(80f, 0f, 0f);
            }
        }
    }
}
