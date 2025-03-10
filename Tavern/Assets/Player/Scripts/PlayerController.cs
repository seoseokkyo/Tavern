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

    public InventoryUI QuickSlotUI_prefab;
    private InventoryUI QuickSlotUI;

    void Start()
    {
        PlayerInventory = GetComponent<InventoryComp>();

        // 인벤토리 사이즈
        PlayerInventory.InventoryInitialize(50);

        // 플레이어의 캔버스 생성
        GameObject PlayerCanvasObj = new GameObject("PlayerCanvas");
        PlayerCanvas = PlayerCanvasObj.AddComponent<Canvas>();
        PlayerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 팝업 형태로 출력되는 인벤토리 UI
        PopupInventoryUI = Instantiate(PopupInventoryUI_Prefab);
        PopupInventoryUI.transform.SetParent(PlayerCanvas.transform, false);

        PopupInventoryUI.SetOwnerInventory(PlayerInventory, 50, 10);

        PopupInventoryUI.gameObject.SetActive(false);
        PopupInventoryUI.enabled = false;

        // 퀵슬롯 인벤토리 UI
        QuickSlotUI = Instantiate(QuickSlotUI_prefab);

        QuickSlotUI.SetOwnerInventory(PlayerInventory);

        QuickSlotUI.transform.SetParent(PlayerCanvas.transform, false); // Canvas의 자식으로 설정 (worldPositionStays = false)

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

        // 뭔가 방법을 찾아야 한다.....
        if (UnityEngine.Input.GetKeyDown("1"))
        {
            Debug.Log("You Pressed \"1?\"");
        }

        if (UnityEngine.Input.GetKeyDown("2"))
        {
            Debug.Log("You Pressed \"2?\"");
        }

        if (UnityEngine.Input.GetKeyDown("3"))
        {
            Debug.Log("You Pressed \"3?\"");
        }

        if (UnityEngine.Input.GetKeyDown("4"))
        {
            Debug.Log("You Pressed \"4?\"");
        }

        if (UnityEngine.Input.GetKeyDown("5"))
        {
            Debug.Log("You Pressed \"5?\"");
        }

        if (UnityEngine.Input.GetKeyDown("6"))
        {
            Debug.Log("You Pressed \"6?\"");
        }

        if (UnityEngine.Input.GetKeyDown("7"))
        {
            Debug.Log("You Pressed \"7?\"");
        }

        if (UnityEngine.Input.GetKeyDown("8"))
        {
            Debug.Log("You Pressed \"8?\"");
        }

        if (UnityEngine.Input.GetKeyDown("9"))
        {
            Debug.Log("You Pressed \"9?\"");
        }

        if (UnityEngine.Input.GetKeyDown("0"))
        {
            Debug.Log("You Pressed \"0?\"");
        }

        // Item Use
    }
}
