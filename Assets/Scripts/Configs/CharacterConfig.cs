using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CharacterConfiguration", menuName = "Configurations/CharacterConfiguration")]
public class CharacterConfig : ScriptableObject
{
    [SerializeField] public float JumpHeight;
    [SerializeField] public float MovementSpeed;
    [SerializeField] public float TimeToJumpMax;
    [SerializeField] public float AccelerationTimeGrounded;
    [SerializeField] public float AccelerationTimeAir;
    [SerializeField] public Vector2 KnockbackStrength;
}
