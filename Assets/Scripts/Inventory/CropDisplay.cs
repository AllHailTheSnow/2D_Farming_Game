using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CropDisplay : MonoBehaviour
{
    [SerializeField] private Image cropImage;
    [SerializeField] private TMP_Text cropAmount;

    public CropController.CropType cropType;

    public void UpdateDisplay()
    {
        CropInfo info = CropController.Instance.GetCropInfo(cropType);

        cropImage.sprite = info.finalCrop;
        cropAmount.text = "x" + info.cropAmount;
    }
}
