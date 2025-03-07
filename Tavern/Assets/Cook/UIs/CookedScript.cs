using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookedScript : MonoBehaviour
{
    public Image cookedViewImage;
    public TextMeshProUGUI cookedItemName;
    public TextMeshProUGUI remainedAmount;

    private ItemData itemData;
    private int remainedItem;

    public GameObject cookingObj;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetCookedItem(ItemData data, int amount)
    {
        itemData = data;
        remainedItem = amount;

        // name
        cookedItemName.text = data.itemName;
        cookedItemName.enabled = true;
        // amount
        remainedAmount.text = amount.ToString();
        remainedAmount.enabled = true;

        // icon
        if(cookedViewImage != null)
        {
            Rect rect = new Rect(0, 0, Mathf.Min(itemData.itemIcon.width, 500), Mathf.Min(itemData.itemIcon.height, 500));
            var temp = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));
            if(temp != null)
            {
                cookedViewImage.sprite = temp;
            }
        }
    }

    // 플레이어가 아이템을 챙길 수 있는지 확인 -> 도구 확인(그릇, 잔..)
    public void CheckCanTakeFood(InventoryComp playerInventroy)
    {
        TakeFood(playerInventroy);
    }

    // 플레이어 인벤토리에 아이템 지급
    private void TakeFood(InventoryComp playerInventroy)
    {
        if(remainedItem > 1)
        {
            remainedItem -= 1;
            remainedAmount.text = remainedItem.ToString();

            ItemBase item = new ItemBase();
            item.SetItemData(itemData);
            playerInventroy.AddItem(ref item);
        }
        else if(remainedItem <= 1)
        {
            remainedItem = 0;

            ItemBase item = new ItemBase();
            item.SetItemData(itemData);
            playerInventroy.AddItem(ref item);

            Interact_CookPot pot = cookingObj.GetComponent<Interact_CookPot>();
            pot.SetSettingNotCooked();
        }
    }
}
