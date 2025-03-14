using System;
using System.Collections.Generic;
using UnityEngine;

public class UseableItem : ItemBase
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UseItem(PlayerController playerController)
    {
        Debug.Log("This is UseableItem");

        bool bDeleteCheck = false;

        foreach (var func in CurrentItemFunctions)
        {
            ItemFunctionArgs tempArgs = new ItemFunctionArgs();
            tempArgs.arg1 = playerController;
            tempArgs.arg2 = func.args.arg2;
            tempArgs.arg3 = func.args.arg3;

            //if (tempArgs.arg3.bDelete)
            //{
            //    bDeleteCheck = true;
            //}

            func.action(tempArgs);
        }

        if(bDeleteCheck)
        {
            OwnerInventory.ConsumeItem(this);
        }
    }
}
