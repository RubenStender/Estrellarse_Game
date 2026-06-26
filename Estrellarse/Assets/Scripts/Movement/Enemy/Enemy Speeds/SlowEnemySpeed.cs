using Estrellarse.Enemy;
using UnityEngine;

public class SlowEnemySpeed : MonoBehaviour, IEnemySpeed
{
    public float MoveSpeed => 2f;
    public float StoppingDistance => 2f;
    public float RotationSpeed => 5f;
}
