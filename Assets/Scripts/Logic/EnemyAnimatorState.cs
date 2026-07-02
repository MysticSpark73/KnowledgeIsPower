namespace Logic
{
    public enum EnemyAnimatorState : byte
    {
        Idle = 0,
        Attack1 = 1,
        Attack2 = 2,
        Moving = 3,
        Death = 4,
        Hurt = 5,
        Win = 6,
        
        Unknown = 255
    }
}