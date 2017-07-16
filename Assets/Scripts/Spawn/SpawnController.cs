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
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private float moveThresholdToSpawn;

    private EventManager eventManager = EventManager.Instance;
    private float currentThreshold;
    private Vector3 lastLevelPosition;
    private Vector3 lastSpawnedPosition;

    public void FixedUpdate()
    {
        if (lastLevelPosition == Vector3.zero)
        {
            lastLevelPosition = playerGameObject.transform.position;
        }

        currentThreshold += playerGameObject.transform.position.x - lastLevelPosition.x;
        lastLevelPosition = playerGameObject.transform.position;

        if (currentThreshold >= moveThresholdToSpawn)
        {
            currentThreshold -= moveThresholdToSpawn;
            SpawnPlatform();
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

        moveThresholdToSpawn = platformComponent.platformWidth; // + Random.Range(0, 5);

        spawnedGameObject = Instantiate(objectToSpawn);

        int yMax = Convert.ToInt32(Math.Floor(Mathf.Clamp(lastSpawnedPosition.y + characterConfig.JumpHeight - 1,
            spawnEnd.position.y,
            spawnStart.position.y)));
        int yPositionToSpawn = Random.Range(Convert.ToInt32(spawnEnd.position.y), yMax);

        int xPos = Convert.ToInt32(Math.Floor(spawnStart.position.x + platformComponent.platformWidth / 2f));

        spawnedGameObject.transform.SetPositionAndRotation(
            new Vector3(xPos, yPositionToSpawn, 0), objectToSpawn.transform.rotation);

        lastSpawnedPosition = spawnedGameObject.transform.position;
        gameObject.transform.Translate(new Vector3(moveThresholdToSpawn, 1, 0));

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
        EnemyBehaviour enemy = evtArgs.ObjectToDestroy.GetComponentInChildren<EnemyBehaviour>();
        if (enemy != null)
        {
            Destroy(enemy.gameObject);
        }

        Destroy(evtArgs.ObjectToDestroy);
    }
}