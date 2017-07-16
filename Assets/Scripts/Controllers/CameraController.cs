using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraConfig camConfig;

    //Public Stuff
    public Transform playerTransform;

    public Vector2 camVelocity;

    private EventManager eventManager = EventManager.Instance;

    void Awake()
    {
        eventManager.RegisterForEvent(EventTypes.PlayerRespawned, OnPlayerRespawn);
    }

    void OnDestroy()
    {
        eventManager.RemoveFromEvent(EventTypes.PlayerRespawned, OnPlayerRespawn);
    }

    private void OnPlayerRespawn(IEvent evt)
    {
        this.transform.position = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        camVelocity = new Vector2(0f, 0f);
        //Check if Out of Bounds
        if (playerTransform.position.x > (this.transform.position.x + camConfig.horizontalDelta))
        {
            float deltaX = playerTransform.position.x - (this.transform.position.x + camConfig.horizontalDelta);
            this.transform.Translate(deltaX, 0f, 0f);
            camVelocity = new Vector2(camVelocity.x + deltaX, camVelocity.y);
        }
        if ((playerTransform.position.x - this.transform.position.x) > camConfig.dampFactor)
        {
            this.transform.Translate(Time.deltaTime * camConfig.camSlowSpeed, 0f, 0f);
            camVelocity = new Vector2(camVelocity.x + (Time.deltaTime * camConfig.camSlowSpeed), camVelocity.y);
        }
        if (playerTransform.position.y > (this.transform.position.y + camConfig.verticalDelta))
        {
            float deltaY = playerTransform.position.y - (this.transform.position.y + camConfig.horizontalDelta);
            this.transform.Translate(0f, deltaY, 0f);
        }
        if ((playerTransform.position.y - this.transform.position.y) > camConfig.dampFactor)
        {
            this.transform.Translate(0f, Time.deltaTime * camConfig.camSlowSpeed, 0f);
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