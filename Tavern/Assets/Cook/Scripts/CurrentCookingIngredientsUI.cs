using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentCookingIngredientsUI : MonoBehaviour
{
    public TMP_Text CookTypeText;

    public Transform ContentTransfrom;

    public CookingIngredientUI CookingIngredientPrefab;

    private List<CookingIngredientUI> UIs = new List<CookingIngredientUI>();

    public void SetData(ECookType CookType, List<WorldItem> CookingItem)
    {
        CookTypeText.text = CookType.ToString();

        foreach (var item in CookingItem)
        {
            CookingIngredientUI temp = Instantiate(CookingIngredientPrefab);

            UIs.Add(temp);

            temp.transform.SetParent(ContentTransfrom);

            temp.CurrentCookType = CookType;

            temp.SetTargetIngredientComp(item.GetComponent<IngredientComp>());
        }
    }

    private void OnDestroy()
    {
        foreach (var temp in UIs)
        {
            if (null != temp)
            {
                Destroy(temp.gameObject);
            }
        }
    }
}
