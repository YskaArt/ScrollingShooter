using UnityEngine;

public class BackgroundMusic : Singleton<BackgroundMusic>
{
   

    private AudioSource audioSource;

    protected override void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

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
