using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlatformConfiguration", menuName = "Configurations/PlatformConfiguration")]
public class SpawnPrefabConfig : ScriptableObject
{
    [SerializeField] public GameObject[] Platforms;
}
