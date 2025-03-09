using Unity.VisualScripting;
using UnityEngine;

public class WellScript : Interactable
{
    public GameObject player;
    public ItemDatas itemDatas;
    InventoryComp playerInventory;

    public override string GetInteractingDescription()
    { 
        return "Press [E] to Fill Water";
    }

    public override void Interact()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if(pc != null)
        {
            playerInventory = pc.GetComponent<InventoryComp>();
            if(playerInventory != null)
            {
                int bucketIdx = CheckHasEmptyBucket(playerInventory);
                int waterBucketIdx = CheckHasWaterBucket(playerInventory);
                if(bucketIdx != -1)         // 빈 양동이
                {
                    for (int i = 0; i < itemDatas.items.Count; i++)
                    {
                        ItemData temp = itemDatas.items[i];
                        if (temp.itemName == "WaterBucket")
                        {
                            ItemBase filledBucket = new ItemBase();
                            filledBucket.SetItemData(temp);
                            filledBucket.CurrentItemData.itemCount = 10;

                            playerInventory.PopItem(bucketIdx);
                            playerInventory.AddItem(ref filledBucket);
                            break;
                        }
                    }
                }
                else if(waterBucketIdx != -1)   // 물 있는 양동이
                {
                    ItemBase temp = playerInventory.CheckItem(waterBucketIdx);
                    temp.CurrentItemData.itemCount = 10;
                    playerInventory.PopItem(waterBucketIdx);
                    playerInventory.AddItem(ref temp);
                }
            }
        }
    }

    private int CheckHasEmptyBucket(InventoryComp inventroy)
    {
        for(int i = 0; i < inventroy.GetInventorySize(); i++)
        {
            ItemBase temp = inventroy.CheckItem(i);
            if(temp != null && temp.CurrentItemData.itemName == "EmptyBucket")
            {
                return i;
            }
        }

        return -1;
    }

    private int CheckHasWaterBucket(InventoryComp inventroy)
    {
        for (int i = 0; i < inventroy.GetInventorySize(); i++)
        {
            ItemBase temp = inventroy.CheckItem(i);
            if (temp != null && temp.CurrentItemData.itemName == "WaterBucket")
            {
                return i;
            }
        }

        return -1;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
