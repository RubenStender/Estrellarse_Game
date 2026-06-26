using Estrellarse.Enemy;
using UnityEngine;

public class MediumEnemySpeed : MonoBehaviour, IEnemySpeed
{
    public float MoveSpeed => 4f;
    public float StoppingDistance => 2f;
    public float RotationSpeed => 8f;
}
