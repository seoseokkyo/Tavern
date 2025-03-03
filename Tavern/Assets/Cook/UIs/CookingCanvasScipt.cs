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
        pc = player.GetComponent<PlayerController>();
        selectedRecipeUI = GetComponentInChildren<SelectedRecipeUI>();
        recipeListUI = GetComponentInChildren<CookingUI>();
    }

    void Update()
    {
        
    }

    public void SetUIs()
    {
        recipeListUI.SetRecipeList();
        selectedRecipeUI.OnSelect("WaterMelon", pc.PlayerInventory);
    }
}
