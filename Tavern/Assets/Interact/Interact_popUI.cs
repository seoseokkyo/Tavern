using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Interact_popUI : Interactable
{
    public GameObject popUI;
    public GameObject interactUI;
    public GameObject player;
    private ModeController modeController;

    private ClickEventTest clickEventTestScript;

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
        // test UI ¶ç¿ò
        popUI.SetActive(true);
        // UI Mode 
        modeController.SetMode(true);
        clickEventTestScript.SetUIActivated(true);
    }
}
