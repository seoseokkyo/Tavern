using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public float moveSpeed;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    public Rigidbody rb;

    public float jumpForce = 3;
    bool grounded = true;

    bool canInteract;

    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.isKinematic = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

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
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        Vector3 targetPos = rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    void Jump()
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
