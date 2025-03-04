
using System.Collections.Generic;
using UnityEngine;

public class CookingUI : MonoBehaviour
{
    public Transform contentTransform;
    public GameObject recipeView;

    List<GameObject> recipeViewList = new List<GameObject>();

    public ItemDatas itemDatas;

    void Start()
    {
        recipeViewList.Clear();
        SetRecipeList();
    }

    void Update()
    {
        
    }
    void ClearList()
    {
        for (int i = recipeViewList.Count - 1; i >= 0; i--)
        {
            GameObject temp = recipeViewList[i];
            recipeViewList.RemoveAt(i);
            Destroy(temp);
        }

        int iCount = contentTransform.childCount;
        for (int i = iCount - 1; i >= 0; i--)
        {
            var temp = contentTransform.GetChild(i);

            if (temp != null)
            {
                var recipeUITemp = temp.GetComponent<RecipeUI>();
                if (recipeUITemp != null)
                {
                    Destroy(temp.gameObject);
                }
            }
        }
    }

    public void SetRecipeList()
    {
        ClearList();

        // 완성품 리스트 가져오기
        for(int i = 0; i < itemDatas.items.Count; i++)
        {
            ItemData temp = itemDatas.items[i];
            
            // 레시피가 있는 것만 레시피 가져옴
            if (temp.recipe != null)
            {
                GameObject prefab = Instantiate(recipeView);
                prefab.transform.SetParent(contentTransform, false);
                RecipeUI tempRecipeUI = prefab.GetComponent<RecipeUI>();

                if (tempRecipeUI != null)
                {
 //                   Debug.Log($"Setting Recipe UI for {temp.itemName}");
                    tempRecipeUI.AddItemData(temp);
                }
                else
                {
                    Debug.Log("RECIPE ADDING ERROR");
                }

                //prefab.SetActive(true);
                recipeViewList.Add(prefab);
            }
        }
    }
}
