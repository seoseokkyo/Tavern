using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using TMPro;

public class CreateTool : Interactable
{
    // 일단은 구조상 UI기능이랑 게임오브젝트 기능 분리가 번거로워서 여기서 둘 다
    // 하려고 했는데... 흠... UI 스크립트랑 얘랑 아예 엮어서 쓰는게 편할듯 기능 파편화랑 스파게티긴 한디

    private GameObject Interacting;

    private CreateToolUI CreateToolUIScript;

    public InventoryComp ToolInventory;
    public GameObject player;
    public GameObject InstantInteractUI;
    protected ModeController modeController;
    protected ClickEventTest clickEventTestScript;

    private bool bTimerOn = false;
    private float Timer;

    public CreateItemType ToolType = CreateItemType.Tool;

    void Start()
    {
        Interacting = Instantiate(InstantInteractUI);

        CreateToolUIScript = Interacting.GetComponent<CreateToolUI>();

        ToolInventory = GetComponent<InventoryComp>();

        // 제일 처음칸은 결과물이 나오는 자리
        var ToolInventoryUI = Interacting.GetComponentInChildren<InventoryUI>();
        if (null != ToolInventoryUI)
        {
            ToolInventory.InventoryInitialize(5 + 1);

            ToolInventoryUI.SetOwnerInventory(ToolInventory, 5 + 1, 1);

            // 결과물 슬롯
            if (null != CreateToolUIScript.ProductSlot)
            {
                var slot = CreateToolUIScript.ProductSlot.GetComponent<ItemSlotUI>();

                slot.SlotIndex = 0;
                slot.OwnerInventory = ToolInventory;
            }
        }

        CreateToolUIScript.ApplyButton.onClick.AddListener(ConditionCheck);
        CreateToolUIScript.ToolType = ToolType;

        Interacting.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (bTimerOn)
        {
            Timer -= Time.deltaTime;
            CreateToolUIScript.TimerText.text = Timer.ToString();

            if (0 >= Timer)
            {
                Timer = 0f;
                CreateToolUIScript.TimerText.text = Timer.ToString();
                bTimerOn = false;
            }
        }
    }
    public override void Interact()
    {
        player = interactPlayer.gameObject;

        TempInitFunction();

        Interacting.transform.SetParent(interactPlayer.PlayerCanvas.transform, false);

        // test UI 띄움
        Interacting.SetActive(true);
        // UI Mode 
        modeController.SetMode(true);
        clickEventTestScript.SetUIActivated(true);
    }

    void TempInitFunction()
    {
        var ExitButton = Interacting.GetComponentInChildren<CookingExitButton>();
        if (null != ExitButton)
        {
            ExitButton.player = interactPlayer.gameObject;
            ExitButton.bTestFlag = true;
        }

        modeController = player.GetComponent<ModeController>();
        clickEventTestScript = Interacting.GetComponent<ClickEventTest>();

        clickEventTestScript.player = player;
        clickEventTestScript.bTestFlag = true;
    }

    bool CheckProductSlotEmpty()
    {
        return (ToolInventory.CheckItem(0) == null);
    }

    void ConditionCheck()
    {
        // 선택한 레시피에 필요한 재료 확인
        // 인벤토리 내에 해당 재료들이 있는지 확인
        // 재료가 충분히 있으면 요구량만큼 소모 후 결과물 생성

        //bool bCheck = ToolInventory.ConsumeItem("Cheese", 5);

        if (false == CheckProductSlotEmpty() || bTimerOn)
        {
            return;
        }

        string CreateTargetItemName = CreateToolUIScript.CurrentSelectedRecipe;
        var ResourceList = ItemManager.Instance.GetCreateItemResourcesListByName(CreateTargetItemName);
        var RcpData = ItemManager.Instance.GetRecipeDataByName(CreateTargetItemName);

        bool bCheck = true;

        foreach (var res in ResourceList)
        {
            if (res.NeedNumber > ToolInventory.CountItemByName(res.ItemName))
            {
                bCheck = false;
            }
        }

        if (bCheck)
        {
            foreach (var res in ResourceList)
            {
                ToolInventory.ConsumeItem(res.ItemName, res.NeedNumber);
            }

            TimerStart(RcpData.CreateNeedTime);
            Invoke("ProductCreate", RcpData.CreateNeedTime);

            Debug.Log("ConsumeItem Success");
        }
        else
        {
            Debug.Log("ConsumeItem Failed");
        }
    }

    public void ProductCreate()
    {
        ItemData CreateItemData = ItemManager.Instance.GetItemDataByName(CreateToolUIScript.CurrentSelectedRecipe);
        ItemBase CreateItem = new ItemBase();
        CreateItem.SetItemData(CreateItemData);

        GameObject instItemUI = Instantiate(CreateToolUIScript.ItemUI);
        instItemUI.transform.SetParent(CreateToolUIScript.ProductSlot.transform);
        instItemUI.transform.position = CreateToolUIScript.ProductSlot.transform.position;
        ItemUI TempItemView = instItemUI.GetComponent<ItemUI>();

        ToolInventory.AddItem(ref CreateItem);

        if (TempItemView != null)
        {
            TempItemView.InitData(CreateItem, CreateToolUIScript.ProductSlot.transform, 0);
        }
    }
    public void TimerStart(float Time)
    {
        Timer = Time;
        bTimerOn = true;
    }

    public override string GetInteractingDescription()
    {
        return "Press [E] To Interact!";
    }
}
