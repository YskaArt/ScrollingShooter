using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Image healthFill;
    private BossController boss;

    void Start()
    {
        boss = FindAnyObjectByType<BossController>();
    }

    void Update()
    {
        if (boss != null && healthFill != null)
        {
            float healthRatio = Mathf.Clamp01((float)boss.CurrentHealth / boss.maxHealth);
            healthFill.fillAmount = healthRatio;
        }
    }
}
