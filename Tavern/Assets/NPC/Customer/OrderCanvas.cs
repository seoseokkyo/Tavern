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
        orderList.Clear();
        RemoveOrderPanel();
    }

    void Update()
    {
        
    }

    public void RemoveOrderPanel()
    {
        orderPanel.SetActive(false);
    }

    public void SetOrderUI(ItemData itemData, int idx)
    {
        stateText.enabled = false;

        orderPanel.gameObject.SetActive(false);

        twoOrder_firstImage.gameObject.SetActive(false);
        twoOrder_secondImage.gameObject.SetActive(false);
        oneOrderImage.gameObject.SetActive(false);


        Rect rect = new Rect(0, 0, Mathf.Min(itemData.itemIcon.width, 500), Mathf.Min(itemData.itemIcon.height, 500));

        var temp = Sprite.Create(itemData.itemIcon, rect, new Vector2(0.5f, 0.5f));

        if (temp != null)
        {
            if(idx == 0)
            {
                oneOrderImage.sprite = temp;
                oneOrderImage.gameObject.SetActive(true);
                oneOrderImage.enabled = true;
            }
            if(idx == 1)
            {
                twoOrder_firstImage.sprite = temp;
                twoOrder_firstImage.gameObject.SetActive(true);
                twoOrder_firstImage.enabled = true;
            }
            if(idx == 2)
            {
                twoOrder_secondImage.sprite = temp;
                twoOrder_secondImage.gameObject.SetActive(true);
                twoOrder_secondImage.enabled = true;
            }
            orderList.Add(itemData);
            orderPanel.gameObject.SetActive(true);
        }
    }
}
