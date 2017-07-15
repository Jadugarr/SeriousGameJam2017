using System;
using UnityEngine;

[Serializable]
public class EnemyConfigObject
{
    [SerializeField] public GameObject EnemyPrefab;
    [SerializeField] public float MovementSpeed;
    [SerializeField] public float JumpHeight;
    [SerializeField] public float TimeToJumpMax;
    [SerializeField] public float AccelerationTimeGrounded;
    [SerializeField] public float AccelerationTimeAir;

    private float gravity;
    private float jumpVelocity;

    public float Gravity
    {
        get { return gravity; }
    }

    public float JumpVelocity
    {
        get { return jumpVelocity; }
    }

    public EnemyConfigObject()
    {
        gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpMax, 2);
        jumpVelocity = Mathf.Abs(gravity) * TimeToJumpMax;
    }
}