using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedDisplay : MonoBehaviour
{
    [SerializeField] CropController.CropType cropType;

    [SerializeField] private Image seedImage;
    [SerializeField] private TMP_Text seedAmount;

    public void UpdateDisplay()
    {
        CropInfo info = CropController.Instance.GetCropInfo(cropType);

        seedImage.sprite = info.seedType;
        seedAmount.text = "x" + info.seedAmount;
    }

    public void SelectSeed()
    {
        PlayerController.Instance.SwitchSeed(cropType);

        UIController.Instance.SwitchSeed(cropType);

        UIController.Instance.inventoryController.OpenClose();
    }
}
