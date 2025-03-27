using UnityEngine;

public class EquipmentItem : ItemBase
{
    protected float Durability = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Durability = CurrentItemData.fOptionValue1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void UseItem(PlayerController playerController)
    {
        playerController.CurrentPlayer.ItemAttachToRightHand(this);

        if (playerController.CurrentEquipmentItem != this)
        {
            Debug.Log($"You Grab This : {CurrentItemData.itemName}");
        }
        else
        {
            foreach (var func in CurrentItemFunctions)
            {
                ItemFunctionArgs tempArgs = new ItemFunctionArgs();
                tempArgs.arg1 = playerController;
                tempArgs.arg2 = func.args.arg2;
                tempArgs.arg3 = func.args.arg3;

                func.action(tempArgs);
            }
        }
    }

    public float AccumulateDurability(float fValue)
    {
        return Durability += fValue;
    }
}
