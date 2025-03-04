using UnityEditor;
using UnityEngine;

public class CookingCanvasScipt : MonoBehaviour
{
    public GameObject player;
    private PlayerController pc;
    private SelectedRecipeUI selectedRecipeUI;
    private CookingUI recipeListUI;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SetUIs()
    {
        pc = player.GetComponent<PlayerController>();
        selectedRecipeUI = GetComponentInChildren<SelectedRecipeUI>();
        recipeListUI = GetComponentInChildren<CookingUI>();

        recipeListUI.SetRecipeList();
        selectedRecipeUI.OnSelect("WaterMelon", pc.PlayerInventory);
    }
}
