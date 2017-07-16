using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CharacterConfiguration", menuName = "Configurations/CharacterConfiguration")]
public class CharacterConfig : ScriptableObject
{
    [SerializeField] public float JumpHeight;
    [SerializeField] public float MovementSpeed0;
    [SerializeField] public float MovementSpeed10;
    [SerializeField] public float MovementSpeed30;
    [SerializeField] public float MovementSpeed60;
    [SerializeField] public float MovementSpeed90;
    [SerializeField] public float MovementSpeed100;
    [SerializeField] public float TimeToJumpMax;
    [SerializeField] public float AccelerationTimeGrounded;
    [SerializeField] public float AccelerationTimeAir;
    [SerializeField] public Vector2 KnockbackStrength;

    [SerializeField] public int progressPerPill;
    [SerializeField] public int progressLossPerFall;
    [SerializeField] public int progressPerEnemySlain;
    [SerializeField] public int progressLossPerHitTaken;

    [SerializeField] public float jumpTime;
}
