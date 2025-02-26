using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float interactionDistance = 2f;

    public GameObject interactionCanvas;
    public GameObject progressBarUI;
    public TextMeshProUGUI interactionText;
    public UnityEngine.UI.Slider interactionProgress;

    private float maxHoldTime = 2f;

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
                interactionText.text = interactable.GetInteractingDescription();
                HandleInteraction(interactable);
                foundInteractable = true;
                progressBarUI.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
            }
        }
        interactionCanvas.SetActive(foundInteractable);
    }

    void HandleInteraction(Interactable interactable)
    {
        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Press:
                if(UnityEngine.Input.GetButtonDown("Interact"))
                {
                    PlayerController player = GetComponentInParent<PlayerController>();
                    if (player != null)
                    {
                        interactable.interactPlayer = player;
                    }
                    
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.Hold:
                KeyCode key = KeyCode.E;
                if (UnityEngine.Input.GetKey(key))
                {
                    interactable.IncreaseHoldtime();
                    if (interactable.GetHoldTime() > maxHoldTime)
                    {
                        interactable.Interact();
                        interactable.ResetHoldTime();
                    }
                }
                else
                {
                    interactable.ResetHoldTime();
                }
                // slider (progressbar) 게이지 상태 업데이트
                interactionProgress.value = interactable.GetHoldTime() / maxHoldTime;
                break;
            case Interactable.InteractionType.UIClick:
                if (UnityEngine.Input.GetButtonDown("Interact"))
                {
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.UIDrag:
                if (UnityEngine.Input.GetButtonDown("Interact"))
                {
                    interactable.Interact();
                }
                break;

        }
    }
}
