using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public string levelToLoad = "Level1"; 
    public string mainMenuScene = "MainMenu"; 

    
    public void PlayGame()
    {
        Time.timeScale = 1f; // Asegura que el tiempo esté normal si venías desde una pausa
        SceneManager.LoadScene(levelToLoad);
    }

    
    public void RestartLevel()
    {
        UIManager.Instance.ResetLivesUI();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();


    }
}
