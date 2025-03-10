using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExitPoint : MonoBehaviour
{
    [Tooltip("The name of the scene exit point")]
    [SerializeField] private string transitionName;

    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            CameraController.Instance.SetPlayerFollowCamera();
            UIFade.Instance.FadeToClear();
        }
    }
}
