using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModeController : MonoBehaviourPunCallbacks
{
    public Camera mainCamera;
    private bool isUIMode = false;

    private PlayerMove playerMoveScript;
    private CameraRotate cameraRotateScript;
    private PlayerInteraction playerinteractionScript;

    private void Awake()
    {
        var OwnerPlayerCon = GetComponentInParent<PlayerController>();

        playerMoveScript = GetComponentInParent<PlayerMove>();
        cameraRotateScript = OwnerPlayerCon.PlayerCamera.GetComponent<CameraRotate>();
        playerinteractionScript = GetComponentInParent<PlayerInteraction>();
    }

    void Start()
    {
        if(photonView.IsMine)
            SetMode(false);  // game 모드용 인풋으로 시작
    }

    void Update()
    {


    }

    private void CheckData()
    {
        if (playerMoveScript == null || cameraRotateScript == null || playerinteractionScript == null)
        {
            var OwnerPlayerCon = GetComponentInParent<PlayerController>();
            playerMoveScript = GetComponentInParent<PlayerMove>();
            cameraRotateScript = OwnerPlayerCon.PlayerCamera.GetComponent<CameraRotate>();
            playerinteractionScript = GetComponentInParent<PlayerInteraction>();
        }
    }

    public void SetMode(bool uiMode)
    {
        if (!photonView.IsMine) return;

        CheckData();

        isUIMode = uiMode;
        playerMoveScript.enabled = !uiMode;
        cameraRotateScript.enabled = !uiMode;
        playerinteractionScript.SetActive(uiMode);
        playerinteractionScript.enabled = !uiMode;

        if(uiMode)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool GetMode()
    {
        return isUIMode;
    }
}
