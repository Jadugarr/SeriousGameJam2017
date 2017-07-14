using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private RectTransform spawnStart;
    [SerializeField] private RectTransform spawnEnd;
    [SerializeField] private SpawnPrefabConfig platformConfig;
    [SerializeField] private float moveThresholdToSpawn;
    [SerializeField] private float speed;

    private EventManager eventManager = EventManager.Instance;
    private float currentThreshold;
    private Vector3 lastPosition;

    public void Update()
    {
        float moveAxis = Input.GetAxis("Horizontal");

        if (moveAxis != 0)
        {
            gameObject.transform.Translate(new Vector3(moveAxis * speed, 0, 0));
        }

        if (lastPosition == Vector3.zero)
        {
            lastPosition = gameObject.transform.position;
        }

        currentThreshold += gameObject.transform.position.x - lastPosition.x;
        lastPosition = gameObject.transform.position;

        if (currentThreshold >= moveThresholdToSpawn)
        {
            SpawnPlatform();
            currentThreshold = 0f;
        }
    }

    public void Awake()
    {
        AddEventListeners();
    }

    public void Destroy()
    {
        RemoveEventListeners();
    }

    private void AddEventListeners()
    {
        eventManager.RegisterForEvent(EventTypes.EnteredDestructionZone, OnDestructionZoneEntered);
    }

    private void RemoveEventListeners()
    {
        eventManager.RemoveFromEvent(EventTypes.EnteredDestructionZone, OnDestructionZoneEntered);
    }

    private void SpawnPlatform()
    {
        GameObject objectToSpawn = platformConfig.Platforms[Random.Range(0, platformConfig.Platforms.Length)];
        float yPositionToSpawn = Random.Range(spawnStart.position.y, spawnEnd.position.y);

        Instantiate(objectToSpawn, new Vector3(spawnStart.position.x, yPositionToSpawn, 0),
            objectToSpawn.transform.rotation);
    }

    private void OnDestructionZoneEntered(IEvent args)
    {
        EnteredDestructionZoneEvent evtArgs = (EnteredDestructionZoneEvent) args;

        if (evtArgs.ObjectToDestroy.CompareTag("Platform"))
        {
            Destroy(evtArgs.ObjectToDestroy);
        }
    }
}