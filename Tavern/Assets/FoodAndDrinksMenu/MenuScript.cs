using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuScript : Interactable
{
    public GameObject player;

    protected ModeController modeController;
    protected ClickEventTest clickEventTestScript;
    
    private GameObject interacting;
    public GameObject instantInteractUI;
    private FaDMenuUI menuUI;

    private void Start()
    {
        interacting = Instantiate(instantInteractUI);
        menuUI = interacting.GetComponent<FaDMenuUI>();
        interacting.SetActive(true);
    }
    public override string GetInteractingDescription()
    {
        return "Press [E] to Set Menu";
    }

    public override void Interact()
    {
        player = interactPlayer.gameObject;

        TempInitFunction();

       // interacting.transform.SetParent(interactPlayer.PlayerCanvas.transform, false);
        interacting.SetActive(true);
        
        menuUI.player = interactPlayer.gameObject;
        menuUI.SetMenuList();

        modeController.SetMode(true);
        clickEventTestScript.SetUIActivated(true);
    }
    void TempInitFunction()
    {
        var ExitButton = interacting.GetComponentInChildren<MenuExitButton>();
        if (null != ExitButton)
        {
            ExitButton.player = interactPlayer.gameObject;
            ExitButton.bTestFlag = true;
        }

        modeController = interactPlayer.GetComponent<ModeController>();
        clickEventTestScript = interacting.GetComponent<ClickEventTest>();

        clickEventTestScript.player = player;
        clickEventTestScript.bTestFlag = true;
    }
}
