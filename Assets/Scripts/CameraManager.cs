using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineMixingCamera mixingCamera;
    public int standardIndex = 0;
    public int bossIndex = 1;
    public int eventIndex = 2;

    public PlayerController player; 

    public void SwitchToStandard()
    {
        mixingCamera.SetWeight(standardIndex, 1f);
        mixingCamera.SetWeight(bossIndex, 0f);
        mixingCamera.SetWeight(eventIndex, 0f);
        player.SetSpecialCameraMode(false);
    }

    public void SwitchToBoss()
    {
        mixingCamera.SetWeight(standardIndex, 0f);
        mixingCamera.SetWeight(bossIndex, 1f);
        mixingCamera.SetWeight(eventIndex, 0f);
        player.SetSpecialCameraMode(false);
    }

    public void SwitchToEvent()
    {
        mixingCamera.SetWeight(standardIndex, 0f);
        mixingCamera.SetWeight(bossIndex, 0f);
        mixingCamera.SetWeight(eventIndex, 1f);
        player.SetSpecialCameraMode(true);
    }
}
