using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OrderCanvas : MonoBehaviour
{
    List<ItemData> orderList = new List<ItemData>();

    public GameObject orderPanel;
    public UnityEngine.UI.Image oneOrderImage;
    public UnityEngine.UI.Image twoOrder_firstImage;
    public UnityEngine.UI.Image twoOrder_secondImage;  

    public TextMeshProUGUI stateText;
    public TextMeshProUGUI timeText;

    void Start()
    {
        this.enabled = true;
        RemoveOrderPanel();
    }

    void Update()
    {
        
    }

    public void RemoveOrderPanel()
    {
        oneOrderImage.enabled = false;
        twoOrder_firstImage.enabled = false;
        twoOrder_secondImage.enabled = false;
        orderPanel.SetActive(false);
    }

    public void Initialize()
    {
        orderList.Clear();
    }

    public void SetOrderUI(ItemData itemData, int idx)
    {
        orderList.Clear();

        stateText.enabled = false;
        
        orderPanel.SetActive(true);
        
        twoOrder_firstImage.enabled = false;
        twoOrder_secondImage.enabled = false;
        oneOrderImage.enabled = false;


        Rect rect = new Rect(0, 0, Mathf.Min(itemData.itemIcon.width, 500), Mathf.Min(itemData.itemIcon.height, 500));

        var temp = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));

        if (temp != null)
        {
            if(idx == 0)
            {
                oneOrderImage.sprite = temp;
                oneOrderImage.enabled = true;
                orderList.Add(itemData);
            }
            if(idx == 1)
            {
                twoOrder_firstImage.sprite = temp;
                twoOrder_firstImage.enabled = true;
                orderList.Add(itemData);
            }
            if(idx == 2)
            {
                twoOrder_secondImage.sprite = temp;
                twoOrder_secondImage.enabled = true;
                orderList.Add(itemData);
            }
        }
    }
}
