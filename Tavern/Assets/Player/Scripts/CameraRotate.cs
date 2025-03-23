using Photon.Pun;
using UnityEngine;

public class CameraRotate : MonoBehaviourPunCallbacks
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform playerRotation;

    float xRotation;
    float yRotation;

    private void Awake()
    {

    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // ModeController 에서 enable, disable 하는거에 따라 Update 함수 호출 제어
        if(PhotonNetwork.IsConnected)
        {
            if(!photonView.IsMine)
            {
                return;
            }
        }

        if (!enabled) return;

        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerRotation.rotation = Quaternion.Euler(0, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
