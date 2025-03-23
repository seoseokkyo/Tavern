using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera cam;
    private PlayerController OwnerPlayerCon;
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

    private void Awake()
    {
        OwnerPlayerCon = GetComponentInParent<PlayerController>();

        cam = OwnerPlayerCon.PlayerCamera;
        if(null == cam)
        {
            Debug.Log("Player Cam Not Init");
        }
    }

    void Update()
    {
        // ModeController 에서 enable, disable 하는거에 따라 Update 함수 호출 제어
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
        if(OwnerPlayerCon != null)
        {
            interactable.interactPlayer = OwnerPlayerCon;
            switch (interactable.interactionType)
            {
                case Interactable.InteractionType.Press:
                    if(UnityEngine.Input.GetButtonDown("Interact"))
                    {
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
}
