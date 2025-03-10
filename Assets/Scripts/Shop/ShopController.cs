using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public ShopSeedDisplay[] seeds;
    public ShopCropDisplay[] crops;

    public void OpenClose()
    {
        if(UIController.Instance.inventoryController.gameObject.activeSelf == false)
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if(gameObject.activeSelf == true)
            {
                foreach (ShopSeedDisplay seed in seeds)
                {
                    seed.UpdateDisplay();
                }

                foreach (ShopCropDisplay crop in crops)
                {
                    crop.UpdateDisplay();
                }
            }
        }
    }
}
