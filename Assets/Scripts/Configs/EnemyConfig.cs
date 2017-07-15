using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "Configurations/EnemyConfiguration")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] public float EnemySpawnChance;
    [SerializeField] public EnemyConfigObject[] PossibleEnemies;
}
