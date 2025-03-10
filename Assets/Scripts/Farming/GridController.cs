using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private Transform minPoint, maxPoint;
    [SerializeField] private GrowBlock GrowBlockPrefab;

    private Vector2Int gridSize;

    public List<BlockRow> blockRows = new List<BlockRow>();

    public LayerMask blockLayer;

    public static GridController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //call the generate grid function
        GenerateGrid();
    }

    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        //round the min and max point positions
        minPoint.position = new Vector3(Mathf.Round(minPoint.position.x), Mathf.Round(minPoint.position.y), 0f);
        maxPoint.position = new Vector3(Mathf.Round(maxPoint.position.x), Mathf.Round(maxPoint.position.y), 0f);

        //set the start position to the min point position + 0.5
        Vector3 startPos = minPoint.position + new Vector3(0.5f, 0.5f, 0f);

        //set the grid size to the max point position - the min point position
        gridSize = new Vector2Int(
            Mathf.RoundToInt(maxPoint.position.x - minPoint.position.x), 
            Mathf.RoundToInt(maxPoint.position.y - minPoint.position.y));

        //loop through the grid size y
        for (int y = 0; y < gridSize.y; y++)
        {
            //add a new block row to the block rows list for each y
            blockRows.Add(new BlockRow());

            //loop through the grid size x
            for (int x = 0; x < gridSize.x; x++)
            {
                //instantiate a new grow block at the start position + x and y position 
                GrowBlock newBlock = Instantiate(GrowBlockPrefab, startPos + new Vector3(x, y, 0f), Quaternion.identity);

                //set the new block parent to the grid controller
                newBlock.transform.SetParent(transform);

                //set the new block grid position to the x and y positions
                newBlock.SetGridPosition(x, y);

                //add the new block to the block rows list at the y index
                blockRows[y].blocks.Add(newBlock);

                //if the new block is overlapping another block then call the block spawn function
                if (Physics2D.OverlapBox(newBlock.transform.position, new Vector2(0.9f, 0.9f), 0f, blockLayer))
                {
                    newBlock.BlockSpawn();
                }

                if(GridInfo.Instance.hasCreatedGrid == true)
                {
                    BlockInfo storedBlocks = GridInfo.Instance.theGrid[y].blockInfo[x];

                    newBlock.currentStage = storedBlocks.currentStage;
                    newBlock.isWatered = storedBlocks.isWatered;
                    newBlock.cropType = storedBlocks.cropType;
                    newBlock.growFailChance = storedBlocks.growFailChance;

                    newBlock.SetSoilSprite();
                    newBlock.UpdateCropSprite();
                }
            }
        }

        if(GridInfo.Instance.hasCreatedGrid == false)
        {
            GridInfo.Instance.CreateGrid();
        }
    }

    public GrowBlock GetBlock(float x, float y)
    {
        //round the x and y positions
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        //subtract the min point position from the x and y positions
        x -= minPoint.position.x;
        y -= minPoint.position.y;

        //set the x and y positions to the rounded x and y positions
        int intX = Mathf.RoundToInt(x);
        int intY = Mathf.RoundToInt(y);

        //if the x and y positions are less than the grid size then return the block at the x and y position
        if (intX < gridSize.x && intY < gridSize.y)
        {
            return blockRows[intY].blocks[intX];
        }

        //return null if the block is not found
        return null;
    }
}

[System.Serializable]
public class BlockRow
{
    //create a list of grow blocks
    public List<GrowBlock> blocks = new List<GrowBlock>();
}
