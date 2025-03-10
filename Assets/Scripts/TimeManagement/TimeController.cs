using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : Singleton<TimeController>
{
    [SerializeField] private float currentTime;
    [SerializeField] private float dayStart, dayEnd;
    [SerializeField] private float timeSpeed = 0.25f;
    [SerializeField] private string dayEndScene;

    private bool timeActive;

    public int currentDay = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = dayStart;

        timeActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeActive == true)
        {
            currentTime += Time.deltaTime * timeSpeed;

            if(currentTime >= dayEnd)
            {
                currentTime = dayEnd;
                EndDay();
            }

            if(UIController.Instance != null)
            {
                UIController.Instance.UpdateTimeText(currentTime);
            }
        }
    }

    public void StartDay()
    {
        timeActive = true;
        currentTime = dayStart;
    }

    public void EndDay()
    {
        timeActive = false;
        currentDay++;
        GridInfo.Instance.GrowCrop();
        PlayerPrefs.SetString("Transition", "WakeUp");
        SceneManager.LoadScene(dayEndScene);
    }
}
