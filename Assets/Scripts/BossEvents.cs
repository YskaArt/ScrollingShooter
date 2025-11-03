using System;

public static class BossEvents
{
    // Evento al cambiar la vida del jefe (vida actual, vida máxima)
    public static event Action<int, int> OnBossHealthChanged;

    // Evento cuando el jefe muere
    public static event Action OnBossDied;

    // Métodos para invocar los eventos (desde BossController)
    public static void NotifyBossHealthChanged(int current, int max)
        => OnBossHealthChanged?.Invoke(current, max);

    public static void NotifyBossDied()
        => OnBossDied?.Invoke();
}
