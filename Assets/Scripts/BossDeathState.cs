using UnityEngine;

public class BossDeathState : BossState
{
    public BossDeathState(BossController boss) : base(boss) { }

    public override void EnterState()
    {
        boss.StartCoroutine(boss.DeathSequence());
    }
}
