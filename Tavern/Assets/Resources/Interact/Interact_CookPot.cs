using UnityEngine;

public class Interact_CookPot : Interactable
{
    public GameObject cookingCanvasUI;
    private CookingCanvasScipt cookingCanvas;
    public GameObject interactUI;
    public GameObject player;
    private ModeController modeController;

    public GameObject cookedUI;
    private CookedScript cookedScript;

    private bool isCooked = false;

    //private ClickExitButton clickExitEventScript;

    private void Start()
    {
        modeController = player.GetComponent<ModeController>();
        cookingCanvas = cookingCanvasUI.GetComponent<CookingCanvasScipt>();
        cookedScript = cookedUI.GetComponent<CookedScript>(); 
    }

    public override string GetInteractingDescription()
    {
        if(isCooked == false)
            return "Press [E] To Cook!";

        return "Press [E] To Take!";
    }

    public override void Interact()
    {
        if(isCooked == false)
        {
            cookingCanvasUI.SetActive(true);
            modeController.SetMode(true);
            cookingCanvas.SetUIs();
        }
        else
        {
            InventoryComp inventory = player.GetComponentInChildren<InventoryComp>();
            if(inventory != null)
                cookedScript.CheckCanTakeFood(inventory);
        }
    }

    public void SetSettingCooked(ItemData itemData, int itemAmount)
    {
        isCooked = true;

        // UI 모드 해제
        modeController.SetMode(false);

        // 기존 레시피 리스트 UI 끄고 CookedUI 띄움
        cookingCanvasUI.SetActive(false);
        cookedUI.SetActive(true);
        cookedScript.SetCookedItem(itemData, itemAmount);

        // UI pop 에서 Press 로 상호작용 타입 변경
        this.interactionType = InteractionType.Press;
    }

    public void SetSettingNotCooked()
    {
        isCooked = false;

        // CookedUI 끄고 상호작용 타입 변경
        cookedUI.SetActive(false);
        this.interactionType = InteractionType.UIPop;
    }
}
