using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private RectTransform spawnStart;
    [SerializeField] private RectTransform spawnEnd;
    [SerializeField] private SpawnPrefabConfig platformConfig;
    [SerializeField] private CharacterConfig characterConfig;
    [SerializeField] private EnemyConfig enemyConfig;
    [SerializeField] private float moveThresholdToSpawn;

    private EventManager eventManager = EventManager.Instance;
    private float currentThreshold;
    private Vector3 lastLevelPosition;
    private Vector3 lastSpawnedPosition;
    private Dictionary<PlatformType, List<GameObject>> platformPool = new Dictionary<PlatformType, List<GameObject>>();

    public void Update()
    {
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
        PlatformComponent platformComponent = objectToSpawn.GetComponent<PlatformComponent>();
        PlatformType typeToSpawn = platformComponent.PlatformType;
        GameObject spawnedGameObject;

        moveThresholdToSpawn = platformComponent.platformWidth + Random.Range(0, 5);

        if (platformPool.ContainsKey(typeToSpawn))
        {
            if (platformPool[typeToSpawn].Count > 0)
            {
                spawnedGameObject = platformPool[typeToSpawn][0];
                spawnedGameObject.SetActive(true);
                platformPool[typeToSpawn].Remove(spawnedGameObject);
            }
            else
            {
                spawnedGameObject = Instantiate(objectToSpawn);
            }
        }
        else
        {
            spawnedGameObject = Instantiate(objectToSpawn);
        }

        int yPositionToSpawn =
            Convert.ToInt32(Math.Floor(Random.Range(
                Mathf.Clamp(lastSpawnedPosition.y + characterConfig.JumpHeight, spawnEnd.position.y,
                    spawnStart.position.y), spawnEnd.position.y)));

        spawnedGameObject.transform.SetPositionAndRotation(new Vector3(Convert.ToInt32(Math.Floor(spawnStart.position.x + platformComponent.platformWidth / 2)), yPositionToSpawn, 0), objectToSpawn.transform.rotation);

        lastSpawnedPosition = spawnedGameObject.transform.position;

        SpawnEnemy(spawnedGameObject);
    }

    private void SpawnEnemy(GameObject spawnedPlatform)
    {
        int randomNumber = Random.Range(1, 101);

        if (randomNumber <= enemyConfig.EnemySpawnChance * 100)
        {
            GameObject spawnPoint = null;

            foreach (Transform currentTransform in spawnedPlatform.transform)
            {
                if (currentTransform.CompareTag("EnemySpawnPoint"))
                {
                    spawnPoint = currentTransform.gameObject;
                    break;
                }
            }

            if (spawnPoint != null)
            {
                EnemyConfigObject enemyConfigObject = enemyConfig
                    .PossibleEnemies[Random.Range(0, enemyConfig.PossibleEnemies.Length)];
                GameObject enemyToSpawn = enemyConfigObject.EnemyPrefab;
                GameObject enemy = Instantiate(enemyToSpawn, spawnPoint.transform);
                EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();
                behaviour.EnemyConfig = enemyConfigObject;
            }
        }
    }

    private void OnDestructionZoneEntered(IEvent args)
    {
        EnteredDestructionZoneEvent evtArgs = (EnteredDestructionZoneEvent) args;

        if (evtArgs.ObjectToDestroy.CompareTag("Platform"))
        {
            EnemyBehaviour enemy = evtArgs.ObjectToDestroy.GetComponentInChildren<EnemyBehaviour>();
            if (enemy != null)
            {
                 Destroy(enemy.gameObject);
            }

            PlatformComponent platformComponent = evtArgs.ObjectToDestroy.GetComponent<PlatformComponent>();
            evtArgs.ObjectToDestroy.SetActive(false);
            if (platformPool.ContainsKey(platformComponent.PlatformType))
            {
                platformPool[platformComponent.PlatformType].Add(evtArgs.ObjectToDestroy);
            }
            else
            {
                platformPool.Add(platformComponent.PlatformType, new List<GameObject>());
                platformPool[platformComponent.PlatformType].Add(evtArgs.ObjectToDestroy);
            }
        }
    }
}