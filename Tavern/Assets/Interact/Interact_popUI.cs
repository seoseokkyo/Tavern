using Unity.Services.Lobbies.Models;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Interact_popUI : Interactable
{
    public GameObject popUI;
    public GameObject interactUI;
    public GameObject player;

    // �̰� �ϴ� Protected�� �������ϴ�. SSK
    protected ModeController modeController;

    // �̰� �ϴ� Protected�� �������ϴ�. SSK
    protected ClickEventTest clickEventTestScript;

    public GameObject InstantInteractUI;

    private void Start()
    {
        modeController = player.GetComponent<ModeController>();
        clickEventTestScript = popUI.GetComponent<ClickEventTest>();
    }

    public override string GetInteractingDescription()
    {
        return "Press [E] To Interact!";
    }

    public override void Interact()
    {
        // test UI ���
        popUI.SetActive(true);
        // UI Mode 
        modeController.SetMode(true);
        clickEventTestScript.SetUIActivated(true);
    }
}
