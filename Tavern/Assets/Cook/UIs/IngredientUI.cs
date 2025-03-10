using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour
{
    public Image IngredientImage;
    public Image IngredientBackGround;

    public TextMeshProUGUI HavingCount;
    public TextMeshProUGUI RequiredCount;

    public ItemData currentItem;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitData(ItemData itemData, Transform parentTransform, int havingCount, int requiredCount)
    {
        currentItem = itemData;
        IngredientBackGround.enabled = true;

        // 필요수량 UI 
        RequiredCount.text = requiredCount.ToString();
        RequiredCount.enabled = true;

        // playerInventroy 재료수량 UI 적용 및 필요수량에 맞는지에 따라 색깔 변경
        HavingCount.text = havingCount.ToString();
        if(havingCount > 0)
        {
            HavingCount.color = new Color(0, 0, 255);
            RequiredCount.enabled = true;
        }
        else
        {
            HavingCount.color = new Color(255, 0, 0);
            RequiredCount.enabled = true;
        }

        if(IngredientImage != null)
        {
            Rect rect = new Rect(0, 0, Mathf.Min(currentItem.itemIcon.width, 500), Mathf.Min(currentItem.itemIcon.height, 500));
            var temp = Sprite.Create(currentItem.itemIcon, rect, new Vector2(0.5f,0.5f));
            if(temp != null)
            {
                IngredientImage.sprite = temp;
               // transform.SetParent(parentTransform);
            }
        }
    }
}
