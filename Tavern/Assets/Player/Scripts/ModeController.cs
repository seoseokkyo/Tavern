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

    private void CheckData()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerMoveScript = player.GetComponent<PlayerMove>();
            playerinteractionScript = player.GetComponent<PlayerInteraction>();
        }
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
            cameraRotateScript = mainCamera.GetComponent<CameraRotate>();
        }
        if(playerMoveScript == null || playerinteractionScript == null || cameraRotateScript == null)
        {
            player = GameObject.FindWithTag("Player");
            mainCamera = Camera.main;
            playerMoveScript = player.GetComponent<PlayerMove>();
            playerinteractionScript = player.GetComponent<PlayerInteraction>();
            cameraRotateScript = mainCamera.GetComponent<CameraRotate>();
        }
    }

    public void SetMode(bool uiMode)
    {
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
