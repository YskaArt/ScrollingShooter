using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public static PlayerHealthUI Instance
    {
        get; private set;
    }

    public Image healthFill;
    private PlayerController player;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // previene múltiples copias
            return;
        }
        Instance = this;
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>(); // intenta recuperarlo si fue respawneado
            return;
        }

        if (healthFill != null)
        {
            float fill = Mathf.Clamp01((float)player.CurrentHealth / player.maxHealth);
            healthFill.fillAmount = fill;
        }
    }
    public void UpdateHealthBar(float normalized)
    {
        healthFill.fillAmount = Mathf.Clamp01(normalized);
    }


}
