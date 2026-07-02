namespace Logic
{
    public interface IAnimationStateReader
    {
        EnemyAnimatorState State { get; }
        void EnteredState(int stateHash);
        void ExitedState(int stateHash);
    }
}