using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            // Si aún no existe una instancia, intentar buscarla en la escena
            if (instance == null)
                instance = FindAnyObjectByType<T>();

            return instance;
        }
    }

    [SerializeField] private bool dontDestroyOnLoad = false;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
