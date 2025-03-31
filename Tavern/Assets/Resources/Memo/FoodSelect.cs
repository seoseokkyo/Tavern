using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

public class FoodSelect : MonoBehaviour
{
    public Button selectButton;

    public UnityEngine.UI.Image icon;
    public Text checkText;

    public bool isSelected = false;
    public ItemData itemData;

    void Start()
    {
        selectButton.onClick.AddListener(OnClickSelectButton);
    }
    public void Initialize(ItemData data)
    {
        itemData = data;

        Rect rect = new Rect(0, 0, Mathf.Min(itemData.itemIcon.width, 500), Mathf.Min(itemData.itemIcon.height, 500));

        var temp = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));

        if (temp != null)
        {
            icon.sprite = temp;
            icon.enabled = true;
        }
    }

    void OnClickSelectButton()
    {
        if (!isSelected)
        {
            isSelected = true;
            selectButton.image.color = Color.green;
            selectButton.enabled = true;
            checkText.text = "V";
            checkText.enabled = true;
        }
        else
        {
            isSelected = false;
            selectButton.image.color = Color.white;
            selectButton.enabled = true;
            checkText.text = "";
            checkText.enabled = true;
        }
    }

}
