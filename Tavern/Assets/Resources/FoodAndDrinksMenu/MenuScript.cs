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
    public FaDMenuUI menuUI;

    private void Start()
    {
        interacting = Instantiate(instantInteractUI);
        menuUI = interacting.GetComponent<FaDMenuUI>();
        menuUI.InitUI();
        menuUI.enabled = true;
        menuUI.modeController = modeController;
        menuUI.clickEventTestScript = clickEventTestScript;
        interacting.SetActive(true);
    }
    public override string GetInteractingDescription()
    {
        return "Press [E] to Set Menu";
    }

    public override void Interact()
    {
        player = interactPlayer.gameObject;
        menuUI.player = interactPlayer.gameObject;

        TempInitFunction();

        // interacting.transform.SetParent(interactPlayer.PlayerCanvas.transform, false);
        if (interacting.activeSelf == false)
        {
            interacting.SetActive(true);
        }

        menuUI.SetMenuList();
        if (menuUI.modeController == null || menuUI.clickEventTestScript == null)
        {
            menuUI.modeController = modeController;
            menuUI.clickEventTestScript = clickEventTestScript;
        }

        modeController.SetMode(true);
        clickEventTestScript.SetUIActivated(true);
    }
    void TempInitFunction()
    {
        var ExitButton = interacting.GetComponentInChildren<CookingExitButton>();
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
