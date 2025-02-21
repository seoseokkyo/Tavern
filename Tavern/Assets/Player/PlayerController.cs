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

    // ��ȣ�ۿ�
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("INTERACT!");
    }

    void Interact()
    {

    }

    // �̵�
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y).normalized;
    }

    void Move()
    {
        float currentMoveSpeed = player.MoveSpeed;

        // ���콺�� �þ� ȸ���Ұ� �����ϸ� �ϴ� Ű���� �Է¿� ���� ȸ�� �ٲ�°� ���ص� �� �� �Ͽ� ���Ƶ�
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
