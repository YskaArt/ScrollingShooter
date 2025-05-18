using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineMixingCamera mixingCamera;
    public GameObject player;

    private int currentCamIndex = 0;
    private int cameraCount = 2;

    private PlayerController playerController;

    private void Start()
    {
        if (player != null)
            playerController = player.GetComponent<PlayerController>();

        SetActiveCamera(0); // Inicia con la cámara estándar
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentCamIndex = (currentCamIndex + 1) % cameraCount;
            SetActiveCamera(currentCamIndex);
        }
    }

    private void SetActiveCamera(int index)
    {
        // Reset all camera weights
        for (int i = 0; i < cameraCount; i++)
        {
            mixingCamera.SetWeight(i, i == index ? 1f : 0f);
        }

        Debug.Log("Cambiando a cámara: " + index);

       
    }
}
