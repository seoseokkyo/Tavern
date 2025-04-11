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
        foreach (GameObject go in foods)
        {
            Destroy(go);
        }

        foods.Clear();

        foreach (string cur in data)
        {
            GameObject prefab = Instantiate(foodIconPrefab);
            prefab.transform.SetParent(foodsContentTransform, false);
            FoodSelect tempUI = prefab.GetComponent<FoodSelect>();
            if (tempUI != null)
            {
                ItemData tempData = FindItemData(cur);
                tempUI.Initialize(tempData);
                tempUI.isSelected = false;
            }
            prefab.SetActive(true);

            RectTransform rectTransform = prefab.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(500f, 500f); 
            }

            foods.Add(prefab);
            Debug.Log($"Added Food Item: {cur}"); 
        }

        extraNoteText.text = extraNoteData;
        extraNoteText.enabled = true;

        Debug.Log($"Total Food Items: {foods.Count}");
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
