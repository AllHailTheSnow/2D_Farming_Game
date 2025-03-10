using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopActivator : MonoBehaviour
{
    private bool canOpen;

    private void Update()
    {
        if(canOpen == true)
        {
            if(Keyboard.current.eKey.wasPressedThisFrame)
            {
                if(UIController.Instance.shopController.gameObject.activeSelf == false)
                {
                    UIController.Instance.shopController.OpenClose();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            canOpen = false;
        }
    }
}
