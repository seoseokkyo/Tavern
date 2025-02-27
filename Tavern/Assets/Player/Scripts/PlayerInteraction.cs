using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float interactionDistance = 2f;

    public GameObject interactionCanvas;
    public GameObject popUICanvas;
    public GameObject progressBarUI;
    public TextMeshProUGUI interactionText;
    public UnityEngine.UI.Slider interactionProgress;

    private float maxHoldTime = 2f;

    private bool deactive = false;
    public void SetActive(bool state)
    {
        deactive = state;
    }

    void Update()
    {
        // ModeController ���� enable, disable �ϴ°ſ� ���� Update �Լ� ȣ�� ����
        if (!enabled) return;

        if (!deactive)
        {
            CheckInteractable();
        }
        else
        {
            interactionCanvas.SetActive(false);
        }

    }

    private void CheckInteractable()
    {
        if(cam)
        {
            Ray ray = cam.ViewportPointToRay(Vector3.one / 2f);
            RaycastHit hit;

            bool foundInteractable = false;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    interactionText.text = interactable.GetInteractingDescription();
                    HandleInteraction(interactable);
                    foundInteractable = true;
                    progressBarUI.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
                }
            }
            interactionCanvas.SetActive(foundInteractable);
        }
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
                // slider (progressbar) ������ ���� ������Ʈ
                interactionProgress.value = interactable.GetHoldTime() / maxHoldTime;
                break;
            case Interactable.InteractionType.UIPop:
                if (UnityEngine.Input.GetButtonDown("Interact"))
                {
                    SetActive(true);
                    interactable.Interact();
                }
                break;
        }
    }
}
