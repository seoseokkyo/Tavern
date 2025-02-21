using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    private float mouseY;
    private float mouseX;


    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        Look();
    }

    void Look()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
    }
}
