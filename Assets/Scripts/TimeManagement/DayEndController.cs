using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DayEndController : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private string wakeUpScene;

    private void Start()
    {
        if(TimeController.Instance != null)
        {
            dayText.text = "- Day " + TimeController.Instance.currentDay + " -";
        }
    }

    private void Update()
    {
        if(Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            TimeController.Instance.StartDay();
            SceneManager.LoadScene(wakeUpScene);
        }
    }
}
