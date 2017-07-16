using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour {

    [SerializeField] private ParallaxConfig paraConfig;

    public Transform[] frontParaArr;
    public Transform[] midParaArr;
    public Transform[] backParaArr;

    public Sprite frontDark;
    public Sprite midDark;
    public Sprite frontTransToNice;
    public Sprite midTransToNice;
    public Sprite frontNice;
    public Sprite midNice;
    public Sprite frontTransToDark;
    public Sprite midTransToDark;


    private EventManager eventManager = EventManager.Instance;
    private int playerDir;
    private GameObject cam;
    private bool darkTheme = true;
    private Transform frontTileMostRight;
    private Transform midTileMostRight;

    public float transTime = 2.5f;
    private bool transToDark = false;
    private bool transToNice = false;
    private float transAcc = 0f;
    private float transMultiply = 35f;

    // Use this for initialization
    void Start () {
        //eventManager.RegisterForEvent(EventTypes.AxisInputEvent, PlayerDirEvent);
        cam = GameObject.FindGameObjectWithTag("Camera");
        eventManager.RegisterForEvent(EventTypes.ProgStepChangeEvent, ProgStepChange);
        frontTileMostRight = frontParaArr[frontParaArr.Length - 1];
        midTileMostRight = midParaArr[midParaArr.Length - 1];
    }

    private void PlayerDirEvent(IEvent playerDirEvent)
    {
        playerDir = ((PlayerDirectionEvent)playerDirEvent).playerDirection;
    }

    private void ProgStepChange(IEvent stepEvent)
    {
        int step = ((ProgStepChangeEvent)stepEvent).progStep;
        if(step > 2 && darkTheme)
        {
            darkTheme = false;
            transToNice = true;
            midTileMostRight.GetComponent<SpriteRenderer>().sprite = midTransToNice;
        } 
        if(step <= 2 && !darkTheme)
        {
            darkTheme = true;
            transToDark = true;
        }
    }

	
	// Update is called once per frame
	void Update () {
        Vector2 currVel = cam.GetComponent<CameraController>().camVelocity;
        //Debug.Log(currVel);
        if (!transToDark && !transToNice)
        {
            for (int i = 0; i < frontParaArr.Length; i++)
            {
                frontParaArr[i].Translate(-currVel.x * paraConfig.Front_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                if (frontParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
                {
                    frontParaArr[i].transform.Translate(80f, 0f, 0f);
                    frontTileMostRight = frontParaArr[i];
                }
            }
            for (int i = 0; i < midParaArr.Length; i++)
            {
                midParaArr[i].Translate(-currVel.x * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                if (midParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
                {
                    midParaArr[i].transform.Translate(80f, 0f, 0f);
                    midTileMostRight = midParaArr[i];
                }
            }
        } else if(transToNice)
        {
            transAcc += Time.deltaTime;
            if(transAcc > transTime)
            {
                transAcc = 0f;
                transToNice = false;
            }
            for (int i = 0; i < midParaArr.Length; i++)
            {
                midParaArr[i].Translate(-transMultiply * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                if (midParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
                {
                    midParaArr[i].transform.Translate(80f, 0f, 0f);
                    midTileMostRight = midParaArr[i];
                    midTileMostRight.GetComponent<SpriteRenderer>().sprite = midNice;
                }
            }
        }
    }
}
