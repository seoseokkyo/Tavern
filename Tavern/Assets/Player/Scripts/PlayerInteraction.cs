using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviourPunCallbacks
{
    private Camera cam;
    private PlayerController OwnerPlayerCon;
    public float interactionDistance = 2f;
    private PlayerUIManager UIManager;

    [HideInInspector]
    public GameObject interactionCanvas;

    [HideInInspector]
    public GameObject popUICanvas;

    [HideInInspector]
    public GameObject progressBarUI;

    [HideInInspector]
    public TextMeshProUGUI interactionText;

    [HideInInspector]
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
        if (null == cam)
        {
            Debug.Log("Player Cam Not Init");
        }

        UIManager = GetComponentInParent<PlayerUIManager>();

        UIManager.OnPlayerUIInitEnd += PlayerInteractionUIInit;
    }

    private void Start()
    {
        
    }

    public void PlayerInteractionUIInit()
    {
        interactionCanvas = UIManager.InteractCanvas;
        popUICanvas = UIManager.popUICanvas;

        var InteractUIPanel = interactionCanvas.transform.Find("InteractCanvasPanel");
        int Count = InteractUIPanel.childCount;
        for (int i = 0; i < Count; i++)
        {
            Transform ChildTransform = InteractUIPanel.GetChild(i);

            if (ChildTransform.name == "HoldTimeProgressBar")
            {
                progressBarUI = ChildTransform.gameObject;
            }
            else if (ChildTransform.name == "Interaction Text")
            {
                interactionText = ChildTransform.GetComponent<TextMeshProUGUI>();
            }
        }

        interactionProgress = progressBarUI.GetComponentInChildren<UnityEngine.UI.Slider>();

        Debug.Log($"progressBarUI : {progressBarUI}, interactionText : {interactionText}, interactionProgress : {interactionProgress}");
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
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        if (cam)
        {
            Ray ray = cam.ViewportPointToRay(Vector3.one / 2f);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

            //TavernGameManager.Instance.debugText += $"\n{ray.origin}";

            bool foundInteractable = false;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    interactionText.text = interactable.GetInteractingDescription();

                    TavernGameManager.Instance.debugText += $"\n{interactionText.text}";

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
        if (OwnerPlayerCon != null)
        {
            interactable.interactPlayer = OwnerPlayerCon;
            switch (interactable.interactionType)
            {
                case Interactable.InteractionType.Press:
                    if (UnityEngine.Input.GetButtonDown("Interact"))
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
