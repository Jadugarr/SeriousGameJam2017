using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CameraConfig", menuName = "Configurations/CameraConfiguration")]
public class CameraConfig : ScriptableObject
{
    [SerializeField] public float horizontalDelta;
    [SerializeField] public float verticalDelta;
    [SerializeField] public float dampFactor;
    [SerializeField] public float camSlowSpeed;    
}
