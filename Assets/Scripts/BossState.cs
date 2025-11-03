using UnityEngine;

public abstract class BossState
{
    protected BossController boss;

    protected BossState(BossController boss)
    {
        this.boss = boss;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }
}
