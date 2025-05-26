using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverMenu;
    public GameObject FirstSpawner;
    public GameObject victoryMenu;
    public GameObject boss;
    public GameObject bossHealthBar;
    public GameObject ScoreFinal;
    public UIManager uIManager;
    private float timer = 0f;
    private bool bossSpawned = false;

    public BossController bossController;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!bossSpawned && timer >= 300f) // 5 minutos
        {
            FirstSpawner.SetActive(false);
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        CameraManager cameraManager = FindAnyObjectByType<CameraManager>();
    if (cameraManager != null)
    {
        cameraManager.StartBossTransition();
    }

    bossSpawned = true;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        uIManager.ShowFinalScore();
        gameOverMenu.SetActive(true);
        ScoreFinal.SetActive(true);
    }

    public void Victory()
    {
        StartCoroutine(Chill());
    }


    private IEnumerator Chill()
    {
        bossController.Die();
        yield return new WaitForSeconds(5f);
  

        Time.timeScale = 0f;
        uIManager.ShowFinalScore();
        victoryMenu.SetActive(true);
        ScoreFinal.SetActive(true);
    }
}
