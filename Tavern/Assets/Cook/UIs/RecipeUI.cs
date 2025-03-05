using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private Button Selectbutton;

    public UnityEngine.UI.Image itemIcon;
    public UnityEngine.UI.Text itemNameText;
    public UnityEngine.UI.Text itemPriceText;

    private List<IngredientAmount> ingredients;
    private ItemData resultItemData;

    SelectedRecipeUI selectedRecipeUI;

    public PlayerController playerController;

    void Start()
    {
        if(Selectbutton != null)
        {
            Selectbutton.onClick.AddListener(OnClickRecipe);
        }
    }

    void Update()
    {
        
    }

    public void AddItemData(ItemData itemData)
    {
        resultItemData = itemData;

        SetName(resultItemData);
        SetPrice(resultItemData);
        SetIcon(resultItemData);
    }

    private void SetName(ItemData itemData)
    {
        itemNameText.text = itemData.itemName;
        itemNameText.enabled = true;
    }

    private void SetPrice(ItemData itemData)
    {
        // °¡°Ý °ª
        //itemPriceText.text = itemData.price;
        itemPriceText.enabled = true;
    }

    private void SetIcon(ItemData itemData)
    {
        Texture2D tempicon = itemData.itemIcon;
        Rect rect = new Rect(0, 0, Mathf.Min(tempicon.width, 500), Mathf.Min(tempicon.height, 500));
        var tempSprite = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));

        itemIcon.sprite = tempSprite;
        itemIcon.enabled = true;
    }

    private void OnClickRecipe()
    {
        selectedRecipeUI.OnSelect(resultItemData.itemName, playerController.PlayerInventory);
    }
}
