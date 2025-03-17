using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class MenuPanelUI : MonoBehaviour
{
    public Button setButton;
    public Text checkText;
    public UnityEngine.UI.Image itemIcon;
    public UnityEngine.UI.Text itemName;
    public UnityEngine.UI.Text itemPrice;
    public Transform iconContentTransform;

    public ItemDatas itemDatas;
    private ItemData itemData;

    public bool isSelected = false;

    public MenuManager menuManagerScript;

    void Start()
    {
        setButton.onClick.AddListener(OnClickSetButton);
    }

    void Update()
    {
        
    }

    public void SetIsSelectedBefore()
    {
        isSelected = true;
        setButton.image.color = Color.green;
        setButton.enabled = true;
        checkText.text = "V";
        checkText.enabled = true;
    }

    void OnClickSetButton()
    {
        if (!isSelected)
        {
            isSelected = true;
            setButton.image.color = Color.green;
            setButton.enabled = true;
            checkText.text = "V";
            checkText.enabled = true;

            if(menuManagerScript != null)
            {
                menuManagerScript.AddMenu(itemData);
            }
        }
        else
        {
            isSelected = false;
            setButton.image.color = Color.white;
            setButton.enabled = true;
            checkText.text = "";
            checkText.enabled = true;
            
            if(menuManagerScript != null)
            {
                menuManagerScript.RemoveMenu(itemData);
            }
        }
    }

    public ItemData GetCurrentItem()
    {
        return itemData;
    }

    public void SetMenuInfo(string name)
    {
        itemData = FindItemData(name);

        // icon
        Texture2D tempicon = itemData.itemIcon;
        Rect rect = new Rect(0, 0, Mathf.Min(tempicon.width, 500), Mathf.Min(tempicon.height, 500));
        var tempSprite = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));

        itemIcon.sprite = tempSprite;
        itemIcon.enabled = true;

        // name
        itemName.text = itemData.itemName;
        itemName.enabled = true;

        // price
        //itemPrice.text = itemData.price;
        //itemPrice.enabled = true;
        setButton.image.color = Color.white;
        setButton.enabled = true;
        checkText.text = "";
        checkText.enabled = true;
    }

    private ItemData FindItemData(string name)
    {
        foreach(ItemData temp in itemDatas.items)
        {
            if(temp.itemName == name)
            {
                return temp;
            }
        }
        return itemDatas.items[0];
    }
}
