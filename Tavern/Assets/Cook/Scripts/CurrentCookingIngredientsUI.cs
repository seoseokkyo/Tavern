using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentCookingIngredientsUI : MonoBehaviour
{
    public TMP_Text CookTypeText;

    public Transform ContentTransfrom;

    public CookingIngredientUI CookingIngredientPrefab;

    private List<CookingIngredientUI> UIs = new List<CookingIngredientUI>();

    [HideInInspector]
    public PlayerController InteractPlayer;

    [HideInInspector]
    public CookInterationObj CookObj;

    private void Start()
    {
        if(InteractPlayer)
        {
            var ModeCon = InteractPlayer.GetComponentInParent<ModeController>();
            if(ModeCon)
            {
                ModeCon.SetMode(true);
            }
        }
    }

    public void SetData(ECookType CookType, List<WorldItem> CookingItem)
    {
        int ChildCount = ContentTransfrom.childCount;

        foreach (var temp in UIs)
        {
            if (null != temp)
            {
                Destroy(temp.gameObject);
            }
        }

        for (int i = 0; i < ChildCount; i++)
        {
            Destroy(ContentTransfrom.GetChild(i));
        }

        CookTypeText.text = CookType.ToString();

        foreach (var item in CookingItem)
        {
            if(item == null)
            {
                continue;
            }

            CookingIngredientUI temp = Instantiate(CookingIngredientPrefab);

            UIs.Add(temp);

            temp.transform.SetParent(ContentTransfrom);

            temp.CurrentCookType = CookType;
            temp.CurrentCookObj = CookObj;

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

    public void ClickedExitButton()
    {
        if (InteractPlayer)
        {
            var ModeCon = InteractPlayer.GetComponentInParent<ModeController>();
            if (ModeCon)
            {
                ModeCon.SetMode(false);
            }
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            ClickedExitButton();
        }
    }
}
