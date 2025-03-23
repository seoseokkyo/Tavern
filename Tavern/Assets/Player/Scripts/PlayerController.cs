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

    public EquipmentItem CurrentEquipmentItem = null;
    public TavernPlayer CurrentPlayer = null;

    public Camera PlayerCamera;
    public Transform CameraTransform;

    private void Awake()
    {
        PlayerCamera.gameObject.transform.SetParent(this.gameObject.transform);
        PlayerCamera.transform.localPosition = CameraTransform.transform.localPosition;
        PlayerCamera.transform.localRotation = CameraTransform.transform.localRotation;
    }

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

        CurrentPlayer = GetComponentInParent<TavernPlayer>();
        if (null == CurrentPlayer)
        {
            Debug.Log("CurrentPlayer Is Null");
        }
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
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("You Pressed \"1?\"");
            PlayerInventory.UseItemByIndex(this, 0);
            QuickSlotUI.SetSlotOutline(0);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("You Pressed \"2?\"");
            PlayerInventory.UseItemByIndex(this, 1);
            QuickSlotUI.SetSlotOutline(1);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("You Pressed \"3?\"");
            PlayerInventory.UseItemByIndex(this, 2);
            QuickSlotUI.SetSlotOutline(2);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("You Pressed \"4?\"");
            PlayerInventory.UseItemByIndex(this, 3);
            QuickSlotUI.SetSlotOutline(3);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad5))
        {
            Debug.Log("You Pressed \"5?\"");
            PlayerInventory.UseItemByIndex(this, 4);
            QuickSlotUI.SetSlotOutline(4);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad6))
        {
            Debug.Log("You Pressed \"6?\"");
            PlayerInventory.UseItemByIndex(this, 5);
            QuickSlotUI.SetSlotOutline(5);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad7))
        {
            Debug.Log("You Pressed \"7?\"");
            PlayerInventory.UseItemByIndex(this, 6);
            QuickSlotUI.SetSlotOutline(6);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha8) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad8))
        {
            Debug.Log("You Pressed \"8?\"");
            PlayerInventory.UseItemByIndex(this, 7);
            QuickSlotUI.SetSlotOutline(7);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha9) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad9))
        {
            Debug.Log("You Pressed \"9?\"");
            PlayerInventory.UseItemByIndex(this, 8);
            QuickSlotUI.SetSlotOutline(8);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("You Pressed \"0?\"");
            PlayerInventory.UseItemByIndex(this, 9);
            QuickSlotUI.SetSlotOutline(9);
        }

        // Item Use
    }
}
