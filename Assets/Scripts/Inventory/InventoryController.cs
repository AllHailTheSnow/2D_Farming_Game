using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public SeedDisplay[] seedDisplays;
    public CropDisplay[] cropDisplays;

    public void OpenClose()
    {
        if(UIController.Instance.shopController.gameObject.activeSelf == false)
        {
            //Open and close the inventory
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
                UpdateDisplay();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void UpdateDisplay()
    {
        foreach(SeedDisplay seed in seedDisplays)
        {
            seed.UpdateDisplay();
        }

        foreach (CropDisplay crop in cropDisplays)
        {
            crop.UpdateDisplay();
        }
    }
}
