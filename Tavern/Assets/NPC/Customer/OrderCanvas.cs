using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OrderCanvas : MonoBehaviour
{
    private List<ItemUI> ItemUIList = new List<ItemUI>();
    public ItemDatas itemDatas;

    public GameObject orderPanel;

    public TextMeshProUGUI stateText;
    public TextMeshProUGUI timeText;

    public Transform ContentTransform;

    public ItemUI ItemUI_Prefab;
    private float ScaleValue = 0.007f;

    void Start()
    {
        stateText.enabled = true;
        timeText.enabled = true;
    }

    void Update()
    {
        
    }

    public void SetOrderUI(List<ItemData> itemList)
    {
        stateText.enabled = false;
        orderPanel.SetActive(true);
        int idx = 0;
        for(int i = 0; i < itemList.Count; i++)
        {
            for (int j = 0; j < itemDatas.items.Count; j++)
            {
                if (itemList[i].itemID == itemDatas.items[j].itemID)
                {
                    idx = i;
                    break;
                }
            }

            ItemUI tempUI = Instantiate(ItemUI_Prefab);
            var tempItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.items[idx]);

            tempUI.InitData(tempItemBase, ContentTransform, i);
//            tempUI.transform.SetParent(ContentTransform);
            tempUI.transform.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
            tempUI.transform.localPosition = new Vector3(tempUI.transform.localPosition.x, tempUI.transform.localPosition.y, 0);
        }
    }
}
