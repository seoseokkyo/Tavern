using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    public Rigidbody rb;

    public float jumpForce = 3;
    bool grounded;

    bool canInteract;

    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ModeController 에서 enable, disable 하는거에 따라 Update 함수 호출 제어
        if (!enabled) return;

        Move();
        animator.SetBool("isMove", moveDir != Vector3.zero);

        if (grounded == true && UnityEngine.Input.GetButtonDown("Jump"))
        {
            Jump();
        }  
    }

    private void Move()
    {
        horizontalInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        verticalInput = UnityEngine.Input.GetAxisRaw("Vertical");
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        transform.position += moveDir.normalized * moveSpeed;
    }

    private void Jump()
    {
        grounded = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = true;
        }
    }
}
