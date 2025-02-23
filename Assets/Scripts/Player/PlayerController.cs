using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;

    private PlayerControls playerControls;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isRunning;
    private float currentMoveSpeed = 1f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        moveSpeed = currentMoveSpeed;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
        RunTrue();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        moveInput = playerControls.Movement.Move.ReadValue<Vector2>();

        anim.SetFloat("MoveX", moveInput.x);
        anim.SetFloat("MoveY", moveInput.y);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + moveInput * (currentMoveSpeed * Time.fixedDeltaTime));
    }

    private void RunTrue()
    {
        isRunning = playerControls.Movement.Run.ReadValue<float>() > 0;

        if(isRunning)
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
}
