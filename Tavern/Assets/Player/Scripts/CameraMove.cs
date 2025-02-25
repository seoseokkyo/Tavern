using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
