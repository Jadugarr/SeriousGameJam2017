using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ParallaxConfig", menuName = "Configurations/ParallaxConfig")]
public class ParallaxConfig : ScriptableObject
{
    [SerializeField] public float Front_ParallaxSpeed;
    [SerializeField] public float Mid_ParallaxSpeed;
    [SerializeField] public float Back_ParallaxSpeed;
    [SerializeField] public float boundaryRight;
    [SerializeField] public float boundaryLeft;

}
