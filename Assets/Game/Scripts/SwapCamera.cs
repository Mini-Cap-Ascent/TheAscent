using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapCamera : MonoBehaviour
{
    public CinemachineVirtualCamera[] virtualCameras; // Your virtual cameras
    private int currentCameraIndex = 0;

    public PlayerCont controll;
   


    private void OnEnable() {
    
        controll = new PlayerCont();
        controll.ChangeCamera.Switch.performed += _ => SwitchCamera();
        controll.Enable();


    }

    private void OnDisable()
    {
        // Remember to disable the action when the script is disabled to clean up
        controll.Disable();
    }

    private void SwitchCamera()
    {
        // Increment the current camera index and wrap around if necessary
        currentCameraIndex = (currentCameraIndex + 1) % virtualCameras.Length;

        // Activate the selected camera and deactivate the others
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            virtualCameras[i].gameObject.SetActive(i == currentCameraIndex);
        }
    }
}
