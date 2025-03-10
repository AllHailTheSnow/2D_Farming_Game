using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : Singleton<CropController>
{
    public enum CropType
    {
        Strawberry,
        Potato,
        Onion,
        Carrot,
        Turnip,
        Cabbage,
        Broccoli,
        Radish
    }

    public List<CropInfo> cropInfo = new List<CropInfo>();

    public CropInfo GetCropInfo(CropType cropToGet)
    {
        int position = -1;

        for (int i = 0; i < cropInfo.Count; i++)
        {
            if (cropInfo[i].cropType == cropToGet)
            {
                position = i;
            }
        }

        if (position >= 0)
        {
            return cropInfo[position]; 
        }
        else
        {
            return null;
        }
    }

    public void UseSeed(CropType seedtoUse)
    {
        foreach(CropInfo info in cropInfo)
        {
            if(info.cropType == seedtoUse)
            {
                info.seedAmount--;
            }
        }
    }

    public void AddCrop(CropType cropToAdd)
    {
        foreach (CropInfo info in cropInfo)
        {
            if (info.cropType == cropToAdd)
            {
                info.cropAmount++;
            }
        }
    }

    public void AddSeed(CropType seedToAdd, int amount)
    {
        foreach (CropInfo info in cropInfo)
        {
            if (info.cropType == seedToAdd)
            {
                info.seedAmount += amount;
            }
        }
    }

    public void RemoveCrop(CropType cropToRemove)
    {
        foreach (CropInfo info in cropInfo)
        {
            if (info.cropType == cropToRemove)
            {
                info.cropAmount = 0;
            }
        }
    }
}

/*--------------------------------------------------------------------------------Resource------------------------------------------------------------------*/

[System.Serializable]
public class CropInfo
{
    public CropController.CropType cropType;
    public Sprite finalCrop, seedType, planted, growStage1, growStage2 , ripe;

    public int seedAmount, cropAmount;

    [Range(0f, 100f)]
    public float growthFailChance;

    public float seedPrice, cropPrice;
}
