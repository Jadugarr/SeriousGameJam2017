using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour {

    [SerializeField] private ParallaxConfig paraConfig;

    public Transform[] frontParaArr;
    public Transform[] midParaArr;
    public Transform nightObject;
    public Transform skyObject;

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
    public ParticleSystem particleSystem;
    public ParticleSystem particleSystem2;
    private bool transToDark = false;
    private bool transToNice = false;
    private float transAcc = 0f;
    private float transMultiply = 35f;

    // Use this for initialization
    void Start () {
        //eventManager.RegisterForEvent(EventTypes.AxisInputEvent, PlayerDirEvent);
        cam = GameObject.FindGameObjectWithTag("Camera");
        particleSystem2.gameObject.SetActive(false);
        eventManager.RegisterForEvent(EventTypes.ProgStepChangeEvent, ProgStepChange);
        frontTileMostRight = frontParaArr[frontParaArr.Length - 1];
        midTileMostRight = midParaArr[midParaArr.Length - 1];
    }

    void OnDestroy()
    {
        eventManager.RemoveFromEvent(EventTypes.ProgStepChangeEvent, ProgStepChange);
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
            particleSystem.Clear();
            particleSystem.gameObject.SetActive(false);
            particleSystem2.gameObject.SetActive(true);
            midTileMostRight.GetComponent<SpriteRenderer>().sprite = midTransToNice;
            frontTileMostRight.GetComponent<SpriteRenderer>().sprite = frontTransToNice;
        } 
        if(step <= 2 && !darkTheme)
        {
            darkTheme = true;
            transToDark = true;
            particleSystem2.Clear();
            particleSystem.gameObject.SetActive(true);
            particleSystem2.gameObject.SetActive(false);
            midTileMostRight.GetComponent<SpriteRenderer>().sprite = midTransToDark;
            frontTileMostRight.GetComponent<SpriteRenderer>().sprite = frontTransToDark;
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
            if (transAcc > transTime)
            {
                transAcc = 0f;
                var main = particleSystem.main;
                transToNice = false;
                nightObject.transform.localPosition = new Vector3(63.3f, 0f, 0f);
                skyObject.transform.localPosition = new Vector3(-.7f, 0f, 0f);
                for (int i = 0; i < midParaArr.Length; i++)
                {
                    midParaArr[i].GetComponent<SpriteRenderer>().sprite = midNice;
                    frontParaArr[i].GetComponent<SpriteRenderer>().sprite = frontNice;
                }
            }
            else
            {
                nightObject.Translate(-transMultiply * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                skyObject.Translate(-transMultiply * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
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
                for (int i = 0; i < frontParaArr.Length; i++)
                {
                    frontParaArr[i].Translate(-transMultiply * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                    if (frontParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
                    {
                        frontParaArr[i].transform.Translate(80f, 0f, 0f);
                        frontTileMostRight = frontParaArr[i];
                        frontTileMostRight.GetComponent<SpriteRenderer>().sprite = frontNice;
                    }
                }
            }
        } else if(transToDark)
        {
            transAcc += Time.deltaTime;
            if (transAcc > transTime)
            {
                transAcc = 0f;
                var main = particleSystem.main;
                transToDark = false;
                skyObject.transform.localPosition = new Vector3(63.3f, 0f, 0f);
                nightObject.transform.localPosition = new Vector3(-.7f, 0f, 0f);
                for(int i = 0;i < midParaArr.Length; i++)
                {
                    midParaArr[i].GetComponent<SpriteRenderer>().sprite = midDark;
                    frontParaArr[i].GetComponent<SpriteRenderer>().sprite = frontDark;
                }
            }
            else
            {
                nightObject.Translate(-transMultiply * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                skyObject.Translate(-transMultiply * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                for (int i = 0; i < midParaArr.Length; i++)
                {
                    midParaArr[i].Translate(-transMultiply * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                    if (midParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
                    {
                        midParaArr[i].transform.Translate(80f, 0f, 0f);
                        midTileMostRight = midParaArr[i];
                        midTileMostRight.GetComponent<SpriteRenderer>().sprite = midDark;
                    }
                }
                for (int i = 0; i < frontParaArr.Length; i++)
                {
                    frontParaArr[i].Translate(-transMultiply * paraConfig.Mid_ParallaxSpeed * Time.deltaTime, 0f, 0f);
                    if (frontParaArr[i].transform.localPosition.x < paraConfig.boundaryLeft)
                    {
                        frontParaArr[i].transform.Translate(80f, 0f, 0f);
                        frontTileMostRight = frontParaArr[i];
                        frontTileMostRight.GetComponent<SpriteRenderer>().sprite = frontDark;
                    }
                }
            }
        }
    }
}
