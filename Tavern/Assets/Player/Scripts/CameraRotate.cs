using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform playerRotation;

    float xRotation;
    float yRotation;

    private void Awake()
    {
        var Transforms = GetComponentsInParent<Transform>();
        foreach (var transform in Transforms)
        {
            if (transform.name == "CameraPos")
            {
                orientation = transform;

                break;
            }
        }

        playerRotation = gameObject.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // ModeController ���� enable, disable �ϴ°ſ� ���� Update �Լ� ȣ�� ����
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
