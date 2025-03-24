using UnityEngine;

public class SelectedItemInfoUI : MonoBehaviour
{
    public UnityEngine.UI.Image itemIcon;
    public UnityEngine.UI.Text itemNameText;
    public UnityEngine.UI.Text itemPriceText;
    public UnityEngine.UI.Text itemInfoText;

    private ItemData resultItemData;

    public void SetResultItem(ItemData itemData)
    {
        resultItemData = itemData;
        SetName(resultItemData.itemName);
        SetIcon(resultItemData);
        SetInfo(resultItemData.itemDescription);
        //SetPrice();
    }

    private void SetName(string name)
    {
        itemNameText.text = name;
        itemNameText.enabled = true;
    }

    private void SetIcon(ItemData itemData)
    {
        Texture2D tempicon = itemData.itemIcon;
        Rect rect = new Rect(0, 0, Mathf.Min(tempicon.width, 500), Mathf.Min(tempicon.height, 500));
        var tempSprite = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));

        itemIcon.sprite = tempSprite;
        itemIcon.enabled = true;
    }

    private void SetPrice(int price)
    {
        itemPriceText.text = price.ToString();
        itemPriceText.enabled = true;   
    }

    private void SetInfo(string str)
    {
        itemInfoText.text = str; 
        itemInfoText.enabled = true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
