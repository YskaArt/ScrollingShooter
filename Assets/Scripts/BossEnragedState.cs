using UnityEngine;

public class BossEnragedState : BossState
{
    private float fireTimer;

    public BossEnragedState(BossController boss) : base(boss)
    {
        fireTimer = boss.FireRate / 2f;
    }

    public override void EnterState()
    {
        Debug.Log("🔥 Boss ha entrado en modo Enraged!");
    }

    public override void UpdateState()
    {
        boss.Move();

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            boss.ExecuteAttackCommands();
            fireTimer = boss.FireRate / 2f;
        }
    }

    public override void ExitState()
    {
        Debug.Log("Boss sale del modo Enraged.");
    }
}
