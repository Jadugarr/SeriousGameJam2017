using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "EnvironmentConfig", menuName = "Configurations/EnvironmentConfig")]
public class EnvironmentConfiguration : ScriptableObject
{
    [SerializeField] public float Gravity;
}
