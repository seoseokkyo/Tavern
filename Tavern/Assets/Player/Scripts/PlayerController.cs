using System;
using System.Threading;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InventoryComp PlayerInventory;

    public GameObject selectedRecipe;
    public GameObject recipe;
    private SelectedRecipeUI selectedRecipeUI;
    private RecipeUI recipeUI;

    void Start()
    {
        PlayerInventory = GetComponent<InventoryComp>();
        PlayerInventory.InventoryInitialize(10);

        selectedRecipeUI = selectedRecipe.GetComponent<SelectedRecipeUI>();
        selectedRecipeUI.GetInventoryFromController(this);
        recipeUI = recipe.GetComponent<RecipeUI>();
        recipeUI.playerController = this;
    }

    void Update()
    {
        if (UnityEngine.Input.GetButtonDown("DropItem"))
        {
            ItemBase item = null;
            for (int i = 0; i < PlayerInventory.GetInventorySize(); i++)
            {
                if(PlayerInventory.CheckItem(i) != null)
                {
                    item = PlayerInventory.PopItem(i);
                    break;
                }
            }    
            
            if(item != null)
            {
                Vector3 Position = gameObject.transform.position;
                Quaternion Rotation = new Quaternion();

                Position += gameObject.transform.forward * 5;

                ItemManager.Instance.ItemSpawn(item, Position, Rotation);
            }
        }
    }
}
