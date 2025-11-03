using UnityEngine;

public class FireStraightCommand : ICommand
{
    private BossController boss;

    public FireStraightCommand(BossController boss)
    {
        this.boss = boss;
    }

    public void Execute()
    {
        boss.FireStraightProjectiles();
    }
}

public class FireHomingCommand : ICommand
{
    private BossController boss;

    public FireHomingCommand(BossController boss)
    {
        this.boss = boss;
    }

    public void Execute()
    {
        boss.FireHomingProjectiles();
    }
}
