using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private RectTransform spawnStart;
    [SerializeField] private RectTransform spawnEnd;
    [SerializeField] private SpawnPrefabConfig platformConfig;
    [SerializeField] private CharacterConfig characterConfig;
    [SerializeField] private float moveThresholdToSpawn;
    [SerializeField] private float speed;

    private EventManager eventManager = EventManager.Instance;
    private float currentThreshold;
    private Vector3 lastLevelPosition;
    private Vector3 lastSpawnedPosition;

    public void Update()
    {
        float moveAxis = Input.GetAxis("Horizontal");

        if (moveAxis != 0)
        {
            gameObject.transform.Translate(new Vector3(moveAxis * speed, 0, 0));
        }

        if (lastLevelPosition == Vector3.zero)
        {
            lastLevelPosition = gameObject.transform.position;
        }

        currentThreshold += gameObject.transform.position.x - lastLevelPosition.x;
        lastLevelPosition = gameObject.transform.position;

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
        if (lastSpawnedPosition == Vector3.zero)
        {
            lastSpawnedPosition = spawnStart.position;
        }

        GameObject objectToSpawn = platformConfig.Platforms[Random.Range(0, platformConfig.Platforms.Length)];
        float yPositionToSpawn =
            Random.Range(
                Mathf.Clamp(lastSpawnedPosition.y + characterConfig.JumpHeight, spawnEnd.position.y,
                    spawnStart.position.y), spawnEnd.position.y);

        GameObject spawnedGameObject = Instantiate(objectToSpawn, new Vector3(spawnStart.position.x, yPositionToSpawn, 0),
            objectToSpawn.transform.rotation);

        lastSpawnedPosition = spawnedGameObject.transform.position;
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