using Estrellarse.Enemy;
using UnityEngine;

public class FastEnemySpeed : MonoBehaviour, IEnemySpeed
{
    public float MoveSpeed => 7f;
    public float StoppingDistance => 1.5f;
    public float RotationSpeed => 12f;
}
