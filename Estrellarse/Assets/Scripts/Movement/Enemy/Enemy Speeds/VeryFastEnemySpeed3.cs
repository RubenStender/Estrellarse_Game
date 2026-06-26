using Estrellarse.Enemy;
using UnityEngine;

public class VeryFastEnemySpeed : MonoBehaviour, IEnemySpeed
{
    public float MoveSpeed => 11f;
    public float StoppingDistance => 1f;
    public float RotationSpeed => 20f;
}
