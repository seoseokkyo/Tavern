using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CustomerAnim : MonoBehaviour
{
    private CustomerScript customer;

    private Transform targetLoc;
    public Transform originLoc;
    bool isCorrect = false;
    bool arrived = false;
    public bool isMoving = false;

    private NavMeshAgent agent;
    public Animator animator;

    float timer = 0;

    private PhotonView photonView;

    private void Awake()
    {

    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        customer = GetComponent<CustomerScript>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if(originLoc != null)
        {
            transform.position = originLoc.position;
            transform.rotation = originLoc.rotation;
        }
    }

    void Update()
    {
        if (isMoving == false)
            return;
        if (!photonView.IsMine || !isMoving) return;

        if (!arrived && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                photonView.RPC("OnArriveRPC", RpcTarget.All);
            }
        }
    }

    public void MoveToLocation(Transform loc)
    {

        targetLoc = loc;
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("MoveToLocationRPC", RpcTarget.All, loc.position);
        }
    }

    [PunRPC]
    void MoveToLocationRPC(Vector3 pos)
    {
        if(targetLoc == null)
        {
            GameObject temp = new GameObject("TargetLoc");
            targetLoc = temp.transform;
        }

        targetLoc.position = pos;

        agent.isStopped = false;
        agent.SetDestination(pos);

        animator.SetBool("isMove", true);
        arrived = false;
        isMoving = true;
    }

    [PunRPC]
    void OnArriveRPC()
    {
        agent.isStopped = true;
        animator.SetBool("isMove", false);
        transform.position = targetLoc.position;

        if (targetLoc != originLoc)
        {
            animator.SetBool("isSitting", true);

            var TempTransforms = targetLoc.GetComponentsInChildren<Transform>();
            foreach (var transform in TempTransforms)
            {
                if (transform.name == "SeatLocation")
                {
                    customer.transform.position = transform.position;
                    customer.transform.rotation = transform.rotation;
                    break;
                }
            }

            customer.SetStateText("Waiting for Response");

            if(photonView.IsMine && PhotonNetwork.IsMasterClient)
            {
                customer.StartCoroutine(customer.WaitForInteraction());
            }
        }

        arrived = true;
        isMoving = false;
    }


    void OnArrive()
    {
        agent.isStopped = true;
        animator.SetBool("isMove", false);
        transform.position = targetLoc.position;

        if (targetLoc != originLoc)
        {
            animator.SetBool("isSitting", true);


            var TempTransforms = targetLoc.GetComponentsInChildren<Transform>();
            foreach (var transform in TempTransforms)
            {
                if (transform.name == "SeatLocation")
                {
                    customer.gameObject.transform.position = transform.position;
                    customer.gameObject.transform.rotation = transform.rotation;

                    break;
                }
            }
        }
    }
    private IEnumerator DelayStartInteraction()
    {
        yield return null;

        customer.SetStateText("Waiting for Response");
        customer.StartCoroutine(customer.WaitForInteraction());
    }

    public void Leave()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LeaveRPC", RpcTarget.All, originLoc.position);
        }
        /*
       agent.isStopped = false;
       StartCoroutine(LeaveTimer());
       targetLoc = originLoc;
       agent.SetDestination(originLoc.position);
       animator.SetBool("isMove", true);

       arrived = false;
       isMoving = true;
        */
    }

    [PunRPC]
    void LeaveRPC(Vector3 pos)
    {
        agent.isStopped = false;
        targetLoc.position = pos;

        agent.SetDestination(pos);
        animator.SetBool("isMove", true);

        arrived = false;
        isMoving = true;

        StartCoroutine(LeaveTimer());
    }

    public void Check(bool correct)
    {
        photonView.RPC("CheckRPC", RpcTarget.All, correct);
        /*
       isCorrect = correct;
       animator.SetBool("isChecking", true);
       if (isCorrect)
       {
           animator.SetBool("isCorrect", true);
       }
       else
       {
           animator.SetBool("isCorrect", false);
       }

        StartCoroutine(ResetCondition());
        */
    }

    [PunRPC]
    void CheckRPC(bool correct)
    {
        isCorrect = correct;
        animator.SetBool("isChecking", true);
        animator.SetBool("isCorrect", correct);

        StartCoroutine(ResetCondition());
    }

    public void Eat()
    {
        //animator.SetBool("isChecking", false);
        photonView.RPC("EatRPC", RpcTarget.All);
    }

    [PunRPC]
    void EatRPC()
    {

    }

    private System.Collections.IEnumerator ResetCondition()
    {
        while (GetTime() < 1f)
        {
            IncreaseTimer();
            yield return null;
        }

        ResetTimer();
        animator.SetBool("isChecking", false);
    }

    private System.Collections.IEnumerator LeaveTimer()
    {
        while (GetTime() < 1f)
        {
            IncreaseTimer();
            yield return null;
        }

        ResetTimer();

        animator.SetBool("isSitting", false);
    }

    private void ResetTimer() => timer = 0f;
    private float GetTime() => timer;
    private void IncreaseTimer() => timer += Time.deltaTime;
}
