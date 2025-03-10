using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BedController : MonoBehaviour
{
    private bool canSleep;

    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Sleep();
        }
    }

    private void Sleep()
    {
        if(canSleep == true)
        {
            if(TimeController.Instance != null)
            {
                TimeController.Instance.EndDay();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            canSleep = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            canSleep = false;
        }
    }
}
