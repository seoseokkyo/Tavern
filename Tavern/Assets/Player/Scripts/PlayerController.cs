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

        // �κ��丮 ������
        PlayerInventory.InventoryInitialize(50);

        // �÷��̾��� ĵ���� ����
        GameObject PlayerCanvasObj = new GameObject("PlayerCanvas");
        PlayerCanvas = PlayerCanvasObj.AddComponent<Canvas>();
        PlayerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // �˾� ���·� ��µǴ� �κ��丮 UI
        PopupInventoryUI = Instantiate(PopupInventoryUI_Prefab);
        PopupInventoryUI.transform.SetParent(PlayerCanvas.transform, false);

        PopupInventoryUI.SetOwnerInventory(PlayerInventory, 50, 10);

        PopupInventoryUI.gameObject.SetActive(false);
        PopupInventoryUI.enabled = false;

        // ������ �κ��丮 UI
        QuickSlotUI = Instantiate(QuickSlotUI_prefab);

        QuickSlotUI.SetOwnerInventory(PlayerInventory);

        QuickSlotUI.transform.SetParent(PlayerCanvas.transform, false); // Canvas�� �ڽ����� ���� (worldPositionStays = false)

        selectedRecipeUI = selectedRecipe.GetComponent<SelectedRecipeUI>();
        selectedRecipeUI.GetInventoryFromController(this);
        recipeUI = recipe.GetComponent<RecipeUI>();
        recipeUI.playerController = this;
    }

    void Update()
    {
        if (UnityEngine.Input.GetButtonDown("DropItem"))
        {
            // �κ��丮���� �������� ������ �ڵ�
            ItemBase item = null;
            for (int i = 0; i < PlayerInventory.GetInventorySize(); i++)
            {
                if (PlayerInventory.CheckItem(i) != null)
                {
                    item = PlayerInventory.PopItem(i);
                    break;
                }
            }

            // ���� �������� ���忡 �����ϴ� �ڵ�
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

        // ���� ����� ã�ƾ� �Ѵ�.....
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("You Pressed \"1?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("You Pressed \"2?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("You Pressed \"3?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("You Pressed \"4?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad5))
        {
            Debug.Log("You Pressed \"5?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad6))
        {
            Debug.Log("You Pressed \"6?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad7))
        {
            Debug.Log("You Pressed \"7?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha8) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad8))
        {
            Debug.Log("You Pressed \"8?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha9) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad9))
        {
            Debug.Log("You Pressed \"9?\"");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("You Pressed \"0?\"");
        }

        // Item Use
    }
}
