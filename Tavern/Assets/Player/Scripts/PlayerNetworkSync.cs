using Photon.Pun;
using UnityEngine;

public class PlayerNetworkSync : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        animator = GetComponentInParent<Animator>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터 보내기
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.linearVelocity);
            stream.SendNext(animator.GetFloat("Speed"));
        }
        else
        {
            // 데이터 받기
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            rb.linearVelocity = (Vector3)stream.ReceiveNext();
            animator.SetFloat("Speed", (float)stream.ReceiveNext());
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            // 네트워크 상의 다른 플레이어 동기화
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10);
        }
    }
}