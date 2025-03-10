using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrowBlock : MonoBehaviour
{
    public enum GrowthStage
    {
        barren,
        ploughed,
        planted,
        growing1,
        growing2,
        ripe
    }

    public GrowthStage currentStage;
    [SerializeField] private SpriteRenderer soilSpriteRenderer;
    [SerializeField] private SpriteRenderer wateredSpriteRender;
    [SerializeField] private SpriteRenderer cropSpriteRenderer;
    [SerializeField] private Sprite soilTilled;
    [SerializeField] private Sprite soilWatered;
    [SerializeField] private Sprite cropPlanted, cropGrowing1, cropGrowing2, cropRipe;

    private Vector2Int gridPosition;

    public CropController.CropType cropType;

    public bool isWatered;
    public bool preventUse;
    public float growFailChance;

    void Start()
    {
        
    }

    void Update()
    {
        //progress the crop growth stage when the n key is pressed in the editor
#if UNITY_EDITOR
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            AdvanceCrop();
        }
#endif

    }

    public void AdvanceStage()
    {
        //progress the crop growth stage
        currentStage = currentStage + 1;

        //if the crop is ripe, set the stage to barren
        if ((int)currentStage >= 6)
        {
            currentStage = GrowthStage.barren;
        }
    }

    public void BlockSpawn()
    {
        //set sprite renderers to null
        soilSpriteRenderer.sprite = null;
        //prevent the block from being used
        preventUse = true;
    }

    public void SetSoilSprite()
    {
        //set the soil sprite based on the current stage
        if (currentStage == GrowthStage.barren)
        {
            soilSpriteRenderer.sprite = null;
            wateredSpriteRender.sprite = null;
        }
        else
        {
            if(isWatered == true)
            {
                soilSpriteRenderer.sprite = soilTilled;
                wateredSpriteRender.sprite = soilWatered;
            }
            else
            {
                soilSpriteRenderer.sprite = soilTilled;
                wateredSpriteRender.sprite = null;
            }
        }

        UpdateGridInfo();
    }

    public void PloughSoil()
    {
        //if the current stage is barren and the block is not being used, set the stage to ploughed
        if (currentStage == GrowthStage.barren && preventUse == false)
        {
            currentStage = GrowthStage.ploughed;
            SetSoilSprite();
        }
    }

    public void WaterSoil()
    {
        //if block is not being prevented from being used, set the block to watered
        if (preventUse == false)
        {
            isWatered = true;
            SetSoilSprite();
        }
    }

    public void PlantCrop(CropController.CropType cropToPlant)
    {
        //if the current stage is ploughed and the block is watered and not being prevented from being used, set the stage to planted
        if (currentStage == GrowthStage.ploughed && isWatered && preventUse == false)
        {
            currentStage = GrowthStage.planted;
            cropType = cropToPlant;
            growFailChance = CropController.Instance.GetCropInfo(cropType).growthFailChance;
            CropController.Instance.UseSeed(cropToPlant);

            UpdateCropSprite();
        }
    }

    public void UpdateCropSprite()
    {
        CropInfo activeCrop = CropController.Instance.GetCropInfo(cropType);

        //set the crop sprite based on the current stage
        switch (currentStage)
        {
            case GrowthStage.planted:
                //cropSpriteRenderer.sprite = cropPlanted;
                cropSpriteRenderer.sprite = activeCrop.planted;
                Debug.Log("Crop Planted");
                break;

            case GrowthStage.growing1:
                //cropSpriteRenderer.sprite = cropGrowing1;
                cropSpriteRenderer.sprite = activeCrop.growStage1;
                Debug.Log("Crop Growing 1");
                break;

            case GrowthStage.growing2:
                //cropSpriteRenderer.sprite = cropGrowing2;
                cropSpriteRenderer.sprite = activeCrop.growStage2;
                Debug.Log("Crop Growing 2");
                break;

            case GrowthStage.ripe:
                //cropSpriteRenderer.sprite = cropRipe;
                cropSpriteRenderer.sprite = activeCrop.ripe;
                Debug.Log("Crop Ripe");
                break;
        }

        UpdateGridInfo();
    }

    public void AdvanceCrop()
    {
        //if the block is watered and not being prevented from being used, advance the crop growth stage
        if (isWatered && preventUse == false)
        {
            if(currentStage == GrowthStage.planted || currentStage == GrowthStage.growing1 || currentStage == GrowthStage.growing2)
            {
                //advance the crop growth stage
                currentStage++;

                //set watered to false
                isWatered = false;
                //set the soil sprite
                SetSoilSprite();
                //update the crop sprite
                UpdateCropSprite();
            }
        }
    }

    public void HarvestCrop()
    {
        //if the current stage is ripe and the block is not being prevented from being used, set the stage to ploughed
        if (currentStage == GrowthStage.ripe && preventUse == false)
        {
            currentStage = GrowthStage.ploughed;
            isWatered = false;
            SetSoilSprite();
            cropSpriteRenderer.sprite = null;
            CropController.Instance.AddCrop(cropType);
        }
    }

    public void SetGridPosition(int x, int y)
    {
        gridPosition = new Vector2Int(x, y);
    }

    private void UpdateGridInfo()
    {
        GridInfo.Instance.UpdateInfo(this, gridPosition.x, gridPosition.y);
    }
}
