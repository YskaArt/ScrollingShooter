using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public CinemachineMixingCamera mixingCamera;
    public int standardIndex = 0;
    public int bossIndex = 1;

    public float transitionDuration = 2f;

    public GameObject boss;
    public GameObject bossHealthBar;

    private void Start()
    {
        // Empezar con la cámara estándar
        SwitchToStandard();
        boss.SetActive(false);
        bossHealthBar.SetActive(false);
    }

    public void SwitchToStandard()
    {
        mixingCamera.SetWeight(standardIndex, 1f);
        mixingCamera.SetWeight(bossIndex, 0f);
    }

    public void StartBossTransition()
    {
        StartCoroutine(TransitionToBossCamera());
    }

    private IEnumerator TransitionToBossCamera()
    {
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            mixingCamera.SetWeight(standardIndex, 1f - t);
            mixingCamera.SetWeight(bossIndex, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegura que los pesos terminen correctamente
        mixingCamera.SetWeight(standardIndex, 0f);
        mixingCamera.SetWeight(bossIndex, 1f);

        // Activar jefe y barra de vida
        boss.SetActive(true);
        bossHealthBar.SetActive(true);
    }
}
