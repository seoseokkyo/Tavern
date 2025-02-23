using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected Player player;
    public Rigidbody rigidBody;
    public PlayerInput playerInput;
    public Animator animator;
    private CharacterController controller;


    public Vector3 moveDir { get; private set; }
    private float mouseX;
    private float playerForward;
    public float mouseSensitivity = 500f;
    public float jumpForce = 10f;
    public bool isJumping;

    void Start()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        animator.SetBool("isMove", moveDir != Vector3.zero);
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    // 상호작용
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("INTERACT!");
    }

    void Interact()
    {

    }

    // 이동
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        // moveDir = transform.forward * input.y + transform.right * input.x;
        moveDir = new Vector3(input.x, 0, input.y).normalized;
        moveDir = controller.transform.TransformDirection(moveDir);
    }

    void Move()
    {
        float currentMoveSpeed = player.MoveSpeed;
        rigidBody.MovePosition(moveDir);
        transform.position += moveDir * currentMoveSpeed * Time.deltaTime;
    //    Quaternion lookAngle = Quaternion.LookRotation(moveDir);
    //    rigidBody.rotation = lookAngle;
    }

    void Look()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, mouseX, 0);
        
    }

    public void OnJump (InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isJumping == false)
        {
            isJumping = true;
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }
    }
}
