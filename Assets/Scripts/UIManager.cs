using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
   

    [Header("Puntuación")]
    public Text scoreText; 
    
    public Text finalScoreText;

    [Header("Pausa")]
    public GameObject pauseMenu;
    private bool isPaused = false;

    [Header("Vidas")]
    public RawImage[] lifeImages;

    public int CurrentScore
    {
        get; private set;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        scoreText.text = ": " + CurrentScore;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void OnPauseButtonPressed()
    {
        TogglePause();
    }

    public void UpdateLivesUI(int currentLives)
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].enabled = i < currentLives;
        }
    }

    public void ResetLivesUI()
    {
        foreach (var img in lifeImages)
        {
            img.enabled = true;
        }
    }
    public void ShowFinalScore()
    {
        finalScoreText.text = ": " + UIManager.Instance.CurrentScore;
    }
}

