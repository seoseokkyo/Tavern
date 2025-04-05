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

    private void Awake()
    {
        if(selectButton != null)
        {
            selectButton.enabled = true;
            selectButton.onClick.AddListener(OnClickSelectButton);
        }
    }

    void Start()
    {

    }
    public void Initialize(ItemData data)
    {
        itemData = data;

        Rect rect = new Rect(0, 0, Mathf.Min(itemData.itemIcon.width, 500), Mathf.Min(itemData.itemIcon.height, 500));
        var temp = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));
        icon.sprite = temp;
        icon.enabled = true;
    }

    public void OnClickSelectButton()
    {
        if (!isSelected)
        {
            isSelected = true;
            if(selectButton.image != null)
            {
                selectButton.image.color = Color.green;

            }
            selectButton.enabled = true;
            if (checkText != null)
            {
                checkText.text = "V";
                checkText.enabled = true;
            }
        }
        else
        {
            isSelected = false;
            if(selectButton.image != null)
            {
                selectButton.image.color = Color.white;
            }
            selectButton.enabled = true;
            if (checkText != null)
            {
                checkText.text = "";
                checkText.enabled = true;
            }
        }
    }

}
