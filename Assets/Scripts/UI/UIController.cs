using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject[] toolbarActiveIcon;
    [SerializeField] private Image seedImage;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private string mainMenuScene;

    public InventoryController inventoryController;
    public ShopController shopController;
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            inventoryController.OpenClose();
        }

#if UNITY_EDITOR
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            shopController.OpenClose();
        }
#endif

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            PauseUnpause();
        }
    }

    public void SwitchTool(int selected)
    {
        //loop through the toolbar active icon and set them to false
        foreach (GameObject icon in toolbarActiveIcon)
        {
            icon.SetActive(false);
        }

        //set the selected toolbar active icon to true
        toolbarActiveIcon[selected].SetActive(true);
    }

    public void UpdateTimeText(float currentTime)
    {
        if(currentTime < 12)
        {
            timeText.text = Mathf.FloorToInt(currentTime) + "AM";
        }
        else if(currentTime < 13)
        {
            timeText.text = "12PM";
        }
        else if(currentTime < 24)
        {
            timeText.text = Mathf.FloorToInt(currentTime - 12) + "PM";
        }
        else if(currentTime < 25)
        {
            timeText.text = "12AM";
        }
        else
        {
            timeText.text = Mathf.FloorToInt(currentTime - 24) + "AM";
        }
    }

    public void SwitchSeed(CropController.CropType crop)
    {
        seedImage.sprite = CropController.Instance.GetCropInfo(crop).seedType;
    }

    public void UpdateMoneyText(float currentMoney)
    {
        moneyText.text = "¥" + currentMoney;
    }

    public void PauseUnpause()
    {
        if(pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);

        Destroy(gameObject);
        Destroy(PlayerController.Instance.gameObject);
        Destroy(GridInfo.Instance.gameObject);
        Destroy(TimeController.Instance.gameObject);
        Destroy(CropController.Instance.gameObject);
        Destroy(CurrencyController.Instance.gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
