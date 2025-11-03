using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    private int maxHealth;

    private void OnEnable()
    {
        BossEvents.OnBossHealthChanged += UpdateHealthUI;
        BossEvents.OnBossDied += HideHealthUI;
    }

    private void OnDisable()
    {
        BossEvents.OnBossHealthChanged -= UpdateHealthUI;
        BossEvents.OnBossDied -= HideHealthUI;
    }

    private void UpdateHealthUI(int current, int max)
    {
        maxHealth = max;
        if (healthFill != null)
            healthFill.fillAmount = Mathf.Clamp01((float)current / max);
    }

    private void HideHealthUI()
    {
        if (healthFill != null)
            healthFill.fillAmount = 0;
    }
}
