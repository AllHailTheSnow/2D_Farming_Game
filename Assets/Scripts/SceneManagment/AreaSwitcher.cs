using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSwitcher : MonoBehaviour
{
    [Tooltip("The name of the scene to load")]
    [SerializeField] private string sceneToLoad;
    [Tooltip("The name of the scene entry point")]
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private Transform spawnPoint;

    private float waitToLoad = 1f;

    private void Start()
    {
        if(PlayerPrefs.HasKey("Transition"))
        {
            if (PlayerPrefs.GetString("Transition") == sceneTransitionName)
            {
                PlayerController.Instance.transform.position = spawnPoint.position;
            }
        }  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetSceneTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        while(waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
        PlayerPrefs.SetString("Transition", sceneTransitionName);
    }
}
