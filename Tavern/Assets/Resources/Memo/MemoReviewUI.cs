using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoReviewUI : MonoBehaviour
{
    public GameObject reviewUIPanel;

    public ItemDatas itemDatas;

    private List<GameObject> foods = new List<GameObject>();
    public GameObject foodIconPrefab;
    public Transform foodsContentTransform;

    public UnityEngine.UI.Text extraNoteText;

    void Start()
    {
        reviewUIPanel.SetActive(false);
    }
    
    public void Initialize(List<string> data, string extraNoteData)
    {
        foreach(string cur in data)
        {
            GameObject prefab = Instantiate(foodIconPrefab);
            prefab.transform.SetParent(foodsContentTransform, false);
            FoodSelect tempUI = prefab.GetComponent<FoodSelect>();
            if (tempUI != null)
            {
                ItemData tempData = FindItemData(name);
                tempUI.Initialize(tempData);
                tempUI.isSelected = false;
            }
            prefab.SetActive(true);
            foods.Add(prefab);
        }

        extraNoteText.text = extraNoteData;
        extraNoteText.enabled = true;
    }
    private ItemData FindItemData(string name)
    {
        foreach (ItemData temp in itemDatas.items)
        {
            if (temp.itemName == name)
            {
                return temp;
            }
        }
        return itemDatas.items[0];
    }

    public void OpenUI()
    {
        reviewUIPanel.SetActive(true);
    }

    public void CloseUI()
    {
        reviewUIPanel.SetActive(false);
    }
}
