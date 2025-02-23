using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float interactionDistance = 2f;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    public UnityEngine.UI.Image interactionProgress;

    void Update()
    {
        CheckInteractable();
    }

    private void CheckInteractable()
    {
        Ray ray = cam.ViewportPointToRay(Vector3.one/2f);
        RaycastHit hit;

        bool foundInteractable = false;
        if(Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            
            if(interactable != null)
            { 
                HandleInteraction(interactable);
                interactionText.text = interactable.GetInteractingDescription();
                foundInteractable = true;
                interactionUI.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
            }
        }

        interactionUI.SetActive(foundInteractable);
    }

    void HandleInteraction(Interactable interactable)
    {
        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Press:
                if(UnityEngine.Input.GetButton("Interact"))
                {
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.Hold:
                if(UnityEngine.Input.GetButton("Interact"))
                {
                    interactable.IncreaseHoldtime();
                    if(interactable.GetHoldTime() > 2f)
                    {
                        interactable.Interact();
                        interactable.ResetHoldTime();
                    }
                }
                else
                {
                    interactable.ResetHoldTime();
                }

                interactionProgress.fillAmount = interactable.GetHoldTime();
                break;
            case Interactable.InteractionType.UIClick:
                if (UnityEngine.Input.GetButton("Interact"))
                {
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.UIDrag:
                if (UnityEngine.Input.GetButton("Interact"))
                {
                    interactable.Interact();
                }
                break;

        }
    }
}
