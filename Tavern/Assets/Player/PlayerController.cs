using System;
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
    void Start()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        animator.SetBool("isMove", moveDir != Vector3.zero);
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

        // 마우스로 시야 회전할거 생각하면 일단 키보드 입력에 따라서 회전 바뀌는건 안해도 될 듯 하여 막아둠
        // LookAt();
        
        transform.position += moveDir * currentMoveSpeed * Time.deltaTime;
    }

    void LookAt()
    {
        if(moveDir != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(moveDir);
            transform.rotation = targetAngle;
        }
    }
}
