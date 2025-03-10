using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridInfo : Singleton<GridInfo>
{
    public bool hasCreatedGrid;
    public List<InfoRow> theGrid = new List<InfoRow>();

    public void CreateGrid()
    {
        hasCreatedGrid = true;

        for(int y = 0; y < GridController.Instance.blockRows.Count; y++)
        {
            theGrid.Add(new InfoRow());

            for(int x = 0; x < GridController.Instance.blockRows[y].blocks.Count; x++)
            {
                BlockInfo newBlockInfo = new BlockInfo();
                theGrid[y].blockInfo.Add(newBlockInfo);
            }
        }
    }

    public void UpdateInfo(GrowBlock theBlock, int xPos, int yPos)
    {
        theGrid[yPos].blockInfo[xPos].currentStage = theBlock.currentStage;
        theGrid[yPos].blockInfo[xPos].isWatered = theBlock.isWatered;
        theGrid[yPos].blockInfo[xPos].cropType = theBlock.cropType;
        theGrid[yPos].blockInfo[xPos].growFailChance = theBlock.growFailChance;
    }

    public void GrowCrop()
    {
        for(int y = 0; y < theGrid.Count; y++)
        {
            for (int x = 0; x < theGrid[y].blockInfo.Count; x++)
            {
                if (theGrid[y].blockInfo[x].isWatered == true)
                {
                    float growthFailTest = Random.Range(0f, 100f);

                    if (growthFailTest > theGrid[y].blockInfo[x].growFailChance)
                    {
                        switch (theGrid[y].blockInfo[x].currentStage)
                        {
                            case GrowBlock.GrowthStage.planted:
                                theGrid[y].blockInfo[x].currentStage = GrowBlock.GrowthStage.growing1;
                                break;

                            case GrowBlock.GrowthStage.growing1:
                                theGrid[y].blockInfo[x].currentStage = GrowBlock.GrowthStage.growing2;
                                break;

                            case GrowBlock.GrowthStage.growing2:
                                theGrid[y].blockInfo[x].currentStage = GrowBlock.GrowthStage.ripe;
                                break;
                        }
                    }

                    theGrid[y].blockInfo[x].isWatered = false;
                }

                if (theGrid[y].blockInfo[x].currentStage == GrowBlock.GrowthStage.ploughed)
                {
                    theGrid[y].blockInfo[x].currentStage = GrowBlock.GrowthStage.barren;
                }
            }
        }
    }

    private void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            GrowCrop();
        }
    }
}

[System.Serializable]
public class BlockInfo
{
    public bool isWatered;
    public GrowBlock.GrowthStage currentStage;
    public CropController.CropType cropType;
    public float growFailChance;
}

[System.Serializable]
public class InfoRow
{
    public List<BlockInfo> blockInfo = new List<BlockInfo>();
}
