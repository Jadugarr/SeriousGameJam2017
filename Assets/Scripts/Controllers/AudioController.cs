using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource[] loopSources;
    [SerializeField] private float blendTime;
    [SerializeField] private AudioSource pillSound;
    [SerializeField] private AudioSource enemyKilledSound;

    private EventManager eventManager = EventManager.Instance;

    private float currentBlendTime = 0f;
    private int currentProgStep = 0;
    private AudioSource currentActiveSource;
    private AudioSource blendSource;
    private AudioSource blendTarget;
    private bool isBlending = false;

    // Use this for initialization
    void Start()
    {
        eventManager.RegisterForEvent(EventTypes.ProgStepChangeEvent, OnNewProgress);
        eventManager.RegisterForEvent(EventTypes.EnemyHit, OnEnemyKilled);
        eventManager.RegisterForEvent(EventTypes.PillTaken, OnPowerPillTaken);
        currentActiveSource = loopSources[0];
    }

    void OnDestroy()
    {
        eventManager.RemoveFromEvent(EventTypes.ProgStepChangeEvent, OnNewProgress);
        eventManager.RemoveFromEvent(EventTypes.EnemyHit, OnEnemyKilled);
        eventManager.RemoveFromEvent(EventTypes.PillTaken, OnPowerPillTaken);
    }

    private void OnPowerPillTaken(IEvent evt)
    {
        pillSound.Play();
    }

    private void OnEnemyKilled(IEvent evt)
    {
        enemyKilledSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlending)
        {
            currentBlendTime += Time.deltaTime;
            blendSource.volume = Mathf.Max(0, 1 - (currentBlendTime / blendTime));
            blendTarget.volume = Mathf.Min(1, currentBlendTime / blendTime);

            if (currentBlendTime >= blendTime)
            {
                currentBlendTime = 0f;
                isBlending = false;
            }
        }
    }

    private void OnNewProgress(IEvent evt)
    {
        ProgStepChangeEvent evtArgs = (ProgStepChangeEvent) evt;
        if (evtArgs.progStep != currentProgStep && evtArgs.progStep < loopSources.Length)
        {
            if (isBlending)
            {
                blendSource.volume = 0f;
                blendTarget.volume = 1f;
                currentBlendTime = 0f;
            }

            blendSource = currentActiveSource;
            blendTarget = loopSources[evtArgs.progStep];
            isBlending = true;

            currentActiveSource = blendTarget;
            currentProgStep = evtArgs.progStep;
        }
    }
}