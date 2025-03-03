using UnityEngine;

public class Interact_CookPot : Interactable
{
    public GameObject cookingCanvasUI;
    private CookingCanvasScipt cookingCanvas;
    public GameObject interactUI;
    public GameObject player;
    private ModeController modeController;

    //private ClickExitButton clickExitEventScript;

    private void Start()
    {
        modeController = player.GetComponent<ModeController>();
        cookingCanvas = cookingCanvasUI.GetComponent<CookingCanvasScipt>();
    }

    public override string GetInteractingDescription()
    {
        return "Press [E] To Cook!";
    }

    public override void Interact()
    {
        cookingCanvasUI.SetActive(true);
        modeController.SetMode(true);
        cookingCanvas.SetUIs();
    }
}
