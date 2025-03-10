using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCropDisplay : MonoBehaviour
{
    public CropController.CropType cropType;

    [SerializeField] private Image cropImage;
    [SerializeField] private TMP_Text amountText, priceText;

    public void UpdateDisplay()
    {
        CropInfo info = CropController.Instance.GetCropInfo(cropType);

        cropImage.sprite = info.finalCrop;
        amountText.text = "x" + info.cropAmount;

        priceText.text = "¥" + info.cropPrice;
    }

    public void SellCrop()
    {
        CropInfo info = CropController.Instance.GetCropInfo(cropType);

        if(info.cropAmount > 0)
        {
            CurrencyController.Instance.AddMoney(info.cropAmount * info.cropPrice);

            CropController.Instance.RemoveCrop(cropType);

            UpdateDisplay();
        }
    }
}
