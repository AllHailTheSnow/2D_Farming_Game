using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyController : Singleton<CurrencyController>
{
    public float currentMoney;

    private void Start()
    {
        UIController.Instance.UpdateMoneyText(currentMoney);
    }

    public void SpendMoney(float amountToSpend)
    {
        currentMoney -= amountToSpend;

        UIController.Instance.UpdateMoneyText(currentMoney);
    }

    public void AddMoney(float amountToAdd)
    {
        currentMoney += amountToAdd;

        UIController.Instance.UpdateMoneyText(currentMoney);
    }

    public bool CheckMoney(float amount)
    {
        if(currentMoney >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
