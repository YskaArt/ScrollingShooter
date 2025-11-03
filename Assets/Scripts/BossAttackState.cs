using UnityEngine;

public class BossAttackState : BossState
{
    private float fireTimer;

    public BossAttackState(BossController boss) : base(boss)
    {
        fireTimer = boss.FireRate;
    }

    public override void EnterState()
    {
        boss.rb.linearVelocity = Vector3.zero;
    }

    public override void UpdateState()
    {
        boss.Move();

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            boss.ExecuteAttackCommands();
            fireTimer = boss.FireRate;
        }

        if (boss.CurrentHealth <= boss.maxHealth / 2 && !(boss.CurrentState is BossEnragedState))
        {
            boss.SwitchState(new BossEnragedState(boss));
        }
    }
}
