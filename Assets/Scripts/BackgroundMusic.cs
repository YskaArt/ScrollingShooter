using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    private AudioSource audioSource;

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Asignar la instancia y hacer que no se destruya al cambiar de escena
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Obtener o agregar un AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Configurar el AudioSource
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        audioSource.Play();
    }

  
}
