using UnityEngine;
using UnityEngine.EventSystems;

public class ModeController : MonoBehaviour
{
    public GameObject player;
    public Camera mainCamera;
    private bool isUIMode = false;

    private PlayerMove playerMoveScript;
    private CameraRotate cameraRotateScript;
    private PlayerInteraction playerinteractionScript;

    void Start()
    {
        playerMoveScript = player.GetComponent<PlayerMove>();
        cameraRotateScript = mainCamera.GetComponent<CameraRotate>();
        playerinteractionScript = player.GetComponent<PlayerInteraction>();
        SetMode(false);  // game 모드용 인풋으로 시작
    }

    void Update()
    {
        
    }

    public void SetMode(bool uiMode)
    {
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
