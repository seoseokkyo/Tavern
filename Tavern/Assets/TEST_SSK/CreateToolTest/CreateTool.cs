using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CreateTool : Interact_popUI
{
    // �ϴ��� ������ UI����̶� ���ӿ�����Ʈ ��� �и��� ���ŷο��� ���⼭ �� ��
    // �Ϸ��� �ߴµ�... ��... UI ��ũ��Ʈ�� ��� �ƿ� ��� ���°� ���ҵ� ��� ����ȭ�� ���İ�Ƽ�� �ѵ�

    private GameObject Interacting;

    private CreateToolUI CreateToolUIScript;

    public InventoryComp ToolInventory;

    public ItemSlotUI ProductSlot;


    void Start()
    {
        Interacting = Instantiate(InstantInteractUI);

        CreateToolUIScript = Interacting.GetComponent<CreateToolUI>();

        ToolInventory = GetComponent<InventoryComp>();

        // ���� ó��ĭ�� ������� ������ �ڸ�
        var ToolInventoryUI = Interacting.GetComponentInChildren<InventoryUI>();
        if (null != ToolInventoryUI)
        {
            ToolInventory.InventoryInitialize(5 + 1);

            ToolInventoryUI.SetOwnerInventory(ToolInventory, 5 + 1, 1);

            // ����� ����
            if (null != ProductSlot)
            {
                ProductSlot.SlotIndex = 0;
                ProductSlot.OwnerInventory = ToolInventory;
            }
        }

        CreateToolUIScript.ApplyButton.onClick.AddListener(ConditionCheck);

        Interacting.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

    }
    public override void Interact()
    {
        player = interactPlayer.gameObject;

        TempInitFunction();

        Interacting.transform.SetParent(interactPlayer.PlayerCanvas.transform, false);

        // test UI ���
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

    void ConditionCheck()
    {
        // ������ �����ǿ� �ʿ��� ��� Ȯ��
        // �κ��丮 ���� �ش� ������ �ִ��� Ȯ��
        // ��ᰡ ����� ������ �䱸����ŭ �Ҹ� �� ����� ����

        bool bCheck = ToolInventory.ConsumeItem("Cheese", 5);

        if (bCheck)
        {
            Debug.Log("ConsumeItem Success");
        }
        else
        {
            Debug.Log("ConsumeItem Failed");
        }
    }
}
