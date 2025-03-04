using System;
using System.Threading;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InventoryComp PlayerInventory;

    public InventoryUI PopupInventoryUI_Prefab;
    private InventoryUI PopupInventoryUI;

    public Canvas PlayerCanvas;
    public GameObject selectedRecipe;
    public GameObject recipe;
    private SelectedRecipeUI selectedRecipeUI;
    private RecipeUI recipeUI;

    void Start()
    {
        PlayerInventory = GetComponent<InventoryComp>();

        // 인벤토리(퀵슬롯) 사이즈
        PlayerInventory.InventoryInitialize(50);

        PopupInventoryUI = Instantiate(PopupInventoryUI_Prefab);
        PlayerCanvas = FindFirstObjectByType<Canvas>();
        PopupInventoryUI.transform.SetParent(PlayerCanvas.transform, false);


        PopupInventoryUI.SetOwnerInventory(PlayerInventory);
        PopupInventoryUI.SlotNum = 50;

        PopupInventoryUI.gameObject.SetActive(false);
        PopupInventoryUI.enabled = false;

        selectedRecipeUI = selectedRecipe.GetComponent<SelectedRecipeUI>();
        selectedRecipeUI.GetInventoryFromController(this);
        recipeUI = recipe.GetComponent<RecipeUI>();
        recipeUI.playerController = this;
    }

    void Update()
    {
        if (UnityEngine.Input.GetButtonDown("DropItem"))
        {
            // 인벤토리에서 아이템을 빼오는 코드
            ItemBase item = null;
            for (int i = 0; i < PlayerInventory.GetInventorySize(); i++)
            {
                if (PlayerInventory.CheckItem(i) != null)
                {
                    item = PlayerInventory.PopItem(i);
                    break;
                }
            }

            // 빼온 아이템을 월드에 스폰하는 코드
            if (item != null)
            {
                Vector3 Position = gameObject.transform.position;
                Quaternion Rotation = new Quaternion();

                Position += gameObject.transform.forward * 5;

                ItemManager.Instance.ItemSpawn(item, Position, Rotation);
            }
        }

        if (UnityEngine.Input.GetButtonDown("InventoryOpen"))
        {
            PopupInventoryUI.enabled = PopupInventoryUI.enabled ? false : true;

            PopupInventoryUI.gameObject.SetActive(PopupInventoryUI.enabled);

            var modeCon = gameObject.GetComponent<ModeController>();

            gameObject.GetComponent<ModeController>().SetMode(!modeCon.GetMode());

        }
    }
}
