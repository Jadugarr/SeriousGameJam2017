using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "Configurations/EnemyConfiguration")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] public GameObject[] PossibleEnemies;
}
