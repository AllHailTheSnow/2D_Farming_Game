using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }

    public void SetSceneTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }
}
