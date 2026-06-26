namespace Estrellarse.Enemy
{
    public interface IEnemySpeed
    {
        float MoveSpeed { get; }
        float StoppingDistance { get; }
        float RotationSpeed { get; }
    }
}