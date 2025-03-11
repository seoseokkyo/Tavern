using UnityEngine;

public class EquipmentItem : ItemBase
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
        if(playerController.CurrentEquipmentItem != this)
        {
            playerController.CurrentEquipmentItem = this;

            Debug.Log($"You Grab This : {CurrentItemData.itemName}");
        }
    }
}
