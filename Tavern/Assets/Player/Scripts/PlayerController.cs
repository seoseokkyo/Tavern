using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    protected Player player;
    public Rigidbody rigidBody;
    public PlayerInput playerInput;
    public Animator animator;

    public Vector3 moveDir { get; private set; }
    public float jumpForce = 10f;
    public bool isJumping;

    void Start()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        animator.SetBool("isMove", moveDir != Vector3.zero);
        CheckIsJumping();
    }

    private void FixedUpdate()
    {
        Move();
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
        moveDir = new Vector3(input.x, 0, input.y).normalized;
    }

    void Move()
    {
        float currentMoveSpeed = player.MoveSpeed;        
        transform.position += moveDir * currentMoveSpeed * Time.deltaTime;
    }

    public void OnJump (InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isJumping == false)
        {
            isJumping = true;
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckIsJumping()
    {                                                                      // LayerMask 3 : Ground 
        isJumping = Physics.Raycast(transform.position, Vector3.down, 0.01f, 3);
    }
}
