using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        //set the player follow camera
        SetPlayerFollowCamera();
    }

    public void SetPlayerFollowCamera()
    {
        //find the cinemachine virtual camera and set the follow target to the player
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }

}
