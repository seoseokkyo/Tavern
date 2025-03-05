using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image ItemViewImage;
    public Image ItemCountbackground;
    public TextMeshProUGUI ItemCount;

    public int ItemIndex;

    public ItemBase CurrentItemBase;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitData(ItemBase itemData, Transform parentTransform, int itemIndex = 0)
    {
        CurrentItemBase = itemData;
        ItemIndex = itemIndex;

        if (CurrentItemBase.CurrentItemData.itemCount > 1)
        {
            ItemCountbackground.enabled = true;

            ItemCount.text = CurrentItemBase.CurrentItemData.itemCount.ToString();
            ItemCount.enabled = true;
        }
        else
        {
            ItemCountbackground.enabled = false;
            ItemCount.enabled = false;
        }

        if (ItemViewImage != null)
        {
            if (CurrentItemBase.CurrentItemData.itemIcon != null && ItemViewImage != null)
            {
                Rect rect = new Rect(0, 0, Mathf.Min(CurrentItemBase.CurrentItemData.itemIcon.width, 500), Mathf.Min(CurrentItemBase.CurrentItemData.itemIcon.height, 500));

                var temp = Sprite.Create(CurrentItemBase.CurrentItemData.itemIcon, rect, new Vector2(0.5f, 0.5f));

                if (temp != null)
                {
                    ItemViewImage.sprite = temp;
                    ItemViewImage.enabled = true;
                    transform.SetParent(parentTransform);
                }
            }
        }
    }

    public void InitData(ItemData itemData, Transform parentTransform, int itemIndex = 0)
    {
        ItemIndex = itemIndex;

        if (itemData.itemCount > 1)
        {
            ItemCountbackground.enabled = true;

            ItemCount.text = itemData.itemCount.ToString();
            ItemCount.enabled = true;
        }
        else
        {
            ItemCountbackground.enabled = false;
            ItemCount.enabled = false;
        }

        if (ItemViewImage != null)
        {
            if (itemData.itemIcon != null && ItemViewImage != null)
            {
                Rect rect = new Rect(0, 0, Mathf.Min(itemData.itemIcon.width, 500), Mathf.Min(itemData.itemIcon.height, 500));

                var temp = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));

                if (temp != null)
                {
                    ItemViewImage.sprite = temp;

                    transform.SetParent(parentTransform);
                }
            }
        }
    }
}
