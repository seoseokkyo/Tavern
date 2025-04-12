using UnityEngine;
using UnityEngine.UI;

public class CookingIngredientUI : MonoBehaviour
{
    [HideInInspector]
    public ECookType CurrentCookType = ECookType.ECookTypeMax;

    public Image IngredientImage;

    public Slider CookSlider;

    private IngredientComp CurrentIngredient;

    [HideInInspector]
    public CookInterationObj CurrentCookObj;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    void Update()
    {
        if(null != CurrentIngredient)
        {
            float fValue = 0.0f;

            switch(CurrentCookType)
            {
                case ECookType.Grill:
                    fValue = CurrentIngredient.GrilledValue;
                    break;
                case ECookType.Boiling:
                    fValue = CurrentIngredient.BoiledValue;
                    break;
                case ECookType.Fry:
                    fValue = CurrentIngredient.FriedValue;
                    break;
            }

            CookSlider.value = fValue;
        }
    }

    public void SetTargetIngredientComp(IngredientComp ingredientComp)
    {
        CurrentIngredient = ingredientComp;

        var ItemSprite = ItemManager.Instance.GetItemSpriteByName(CurrentIngredient.CurrentWorldItem.item.CurrentItemData.itemName);

        IngredientImage.sprite = ItemSprite;
    }

    public void OnClickTakeOutButton()
    {
        CurrentCookObj.IngredientTakeOut(CurrentIngredient.CurrentWorldItem);
    }
}
