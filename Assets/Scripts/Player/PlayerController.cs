using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private Transform toolIndicator;
    [SerializeField] private float toolWaitTime = 0.8f;
    [SerializeField] private float toolRange;

    private PlayerControls playerControls;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isPaused;
    private bool isRunning;
    private float currentMoveSpeed;
    private Vector2 mousePos;
    private Vector2 posDif;
    private float toolWaitCounter;
    //private int currentToolIndex = 0;

    //enum for the tools
    public enum ToolType
    {
        Hoe,
        WatteringCan,
        Seeds,
        Shovel
    }

    public ToolType currentTool;
    public CropController.CropType seedCropType;

    protected override void Awake()
    {
        base.Awake();

        //initialize the player controls and the rigidbody and animator
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //set the current speed to the move speed
        currentMoveSpeed = moveSpeed;

        //subscribe to the events of the player controls
        playerControls.Action.Interact.performed += _ => UseTools();
        playerControls.Action.SwapTool.performed += _ => SwapTool();
        playerControls.Action.Keyboard.performed += ctx => ToggeleActiveTool((int)ctx.ReadValue<float>());

        //set the current tool to the first tool in the enum
        UIController.Instance.SwitchTool((int)currentTool);

        UIController.Instance.SwitchSeed(seedCropType);

    }

    private void OnEnable()
    {
        //enable the player controls
        playerControls.Enable();
    }

    private void Update()
    {
        //get the player input and run the run method and the tool indicator method and the action direction method
        PlayerInput();
        RunTrue();
        ToolIndicator();
        ActionDirection();
        Paused();
    }

    private void FixedUpdate()
    {
        //run the move method
        Move();
    }

    /*--------------------------------------------------------------------Movement Methods-------------------------------------------------------------------------*/

    private void PlayerInput()
    {
        //set the move input to the player input value
        moveInput = playerControls.Movement.Move.ReadValue<Vector2>();

        //set the animator parameters for the movex and movey movement
        anim.SetFloat("MoveX", moveInput.x);
        anim.SetFloat("MoveY", moveInput.y);
    }

    private void Move()
    {
        if(isPaused == true)
        {
            rb.velocity = Vector2.zero;

            return;
        }
        else
        {
            //if the tool wait counter is greater than 0 then the player cannot move
            if (toolWaitCounter > 0)
            {
                //decrease the tool wait counter and set the velocity to 0
                toolWaitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                //if the tool wait counter is less than 0 then the player can move
                //set the velocity to the move input times the current move speed times the time delta time
                rb.MovePosition(rb.position + moveInput * (currentMoveSpeed * Time.deltaTime));
            }
        }        
    }

    private void RunTrue()
    {
        //if the player is running then set the animator parameter to true and set the current move speed to the run speed
        isRunning = playerControls.Movement.Run.ReadValue<float>() > 0;

        if (isRunning)
        {
            anim.SetBool("isRunning", true);
            currentMoveSpeed = runSpeed;
        }
        else
        {
            anim.SetBool("isRunning", false);
            currentMoveSpeed = moveSpeed;
        }
    }

    private void ActionDirection()
    {
        //get the mouse position and the difference between the mouse position and the player position
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        posDif = mousePos - rb.position;
        //set the animator parameters for the horizontal and vertical movement
        anim.SetFloat("Horizontal", posDif.x);
        anim.SetFloat("Vertical", posDif.y);
    }

    private void Paused()
    {
        if (UIController.Instance != null)
        {
            if (UIController.Instance.inventoryController != null)
            {
                if (UIController.Instance.inventoryController.gameObject.activeSelf == true)
                {
                    isPaused = true;
                }
                else
                {
                    isPaused = false;
                }

                if (UIController.Instance.shopController.gameObject.activeSelf == true)
                {
                    isPaused = true;
                }
                else
                {
                    isPaused = false;
                }

                if (UIController.Instance.pauseMenu.gameObject.activeSelf == true)
                {
                    isPaused = true;
                }
                else
                {
                    isPaused = false;
                }
            }
        }
    }

    /*---------------------------------------------------------------------Farming Methods--------------------------------------------------------------------------*/

    private void ToolIndicator()
    {
        if(GridController.Instance != null)
        {
            //get the mouse position and set the tool indicator position to the mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            toolIndicator.position = mousePos;

            //if the distance between the tool indicator and the player is greater than the tool range
            //then set the direction to the tool indicator position minus the player position
            //then set the direction to the normalized direction times the tool range
            //then set the tool indicator position to the player position plus the direction
            if (Vector3.Distance(toolIndicator.position, transform.position) > toolRange)
            {
                Vector2 direction = toolIndicator.position - transform.position;
                direction = direction.normalized * toolRange;
                toolIndicator.position = transform.position + new Vector3(direction.x, direction.y, 0f);
            }

            //set the tool indicator position to the floored x and y position plus 0.5f for grid snapping
            toolIndicator.position = new Vector3(Mathf.FloorToInt(toolIndicator.position.x) + 0.5f, Mathf.FloorToInt(toolIndicator.position.y) + 0.5f, 0f);
        }
        else
        {
            toolIndicator.position = new Vector3(0f, 0f, -20f);
        }
    }

    private void UseTools()
    {
        if(GridController.Instance != null)
        {
            if(isPaused == true)
            {
                return;
            }
            else
            {
                //get the grow block and set the tool wait counter to the tool wait time
                GrowBlock block = null;
                toolWaitCounter = toolWaitTime;

                block = GridController.Instance.GetBlock(toolIndicator.position.x - 0.5f, toolIndicator.position.y - 0.5f);


                //if the block is not null then switch the current tool and use the tool
                if (block != null)
                {
                    switch (currentTool)
                    {
                        case ToolType.Hoe:
                            anim.SetTrigger("useHoe");
                            block.PloughSoil();
                            break;

                        case ToolType.WatteringCan:
                            anim.SetTrigger("useWateringCan");
                            block.WaterSoil();
                            break;

                        case ToolType.Seeds:
                            if (CropController.Instance.GetCropInfo(seedCropType).seedAmount > 0)
                            {
                                block.PlantCrop(seedCropType);
                                //CropController.Instance.UseSeed(seedCropType);
                            }
                            break;

                        case ToolType.Shovel:
                            anim.SetTrigger("useShovel");
                            block.HarvestCrop();
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }

    private void SwapTool()
    {
        //switch the current tool and set the current tool to the next tool
        currentTool++;

        //if the current tool is greater than or equal to 4 then set the current tool to the hoe
        if ((int)currentTool >= 4)
        {
            currentTool = ToolType.Hoe;
        }
    }

    private void ToggeleActiveTool(int toolIndex)
    {
        //set the current tool to the tool index minus 1
        currentTool = (ToolType)toolIndex - 1;
        UIController.Instance.SwitchTool((int)currentTool);
    }

    public void SwitchSeed(CropController.CropType newSeed)
    {
        seedCropType = newSeed;
    }

    
}
