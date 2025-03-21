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

    void Start()
    {
        customer = GetComponent<CustomerScript>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isMoving == false)
                return;

        if (!arrived && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
           if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
           {
               OnArrive();
               arrived = true;
               isMoving = false;
           }
        }
    }

    public void MoveToLocation(Transform loc)
    {
        targetLoc = loc;
        agent.SetDestination(loc.position);
        animator.SetBool("isMove", true);
        arrived = false;
        isMoving = true;
    }

    void OnArrive()
    {
        agent.isStopped = true;
        animator.SetBool("isMove", false);

        transform.position = targetLoc.position;

        if (targetLoc != originLoc)
        {
            animator.SetBool("isSitting", true);
            animator.SetBool("isMove", false);
            customer.Initialize();
            customer.DecideOrder();
        }
    }

    public void Leave()
    {
        StartCoroutine(LeaveTimer());

        targetLoc = originLoc;
        agent.SetDestination(originLoc.position);
        animator.SetBool("isMove", true);

        arrived = false;
        isMoving = true;
    }

    public void Check(bool correct)
    {
        isCorrect = correct;
        animator.SetBool("isChecking", true);
        if(isCorrect)
        {
            animator.SetBool("isCorrect", true);
        }
        else
        {
            animator.SetBool("isCorrect", false);
        }
        
        StartCoroutine(ResetCondition());
    }

    public void Eat()
    {
        //animator.SetBool("isChecking", false);
    }

    private System.Collections.IEnumerator ResetCondition()
    {
        while(GetTime() < 2f)
        {
            IncreaseTimer();
            yield return null;
        }

        ResetTimer();
        animator.SetBool("isChecking", false);
        animator.SetBool("isMove", false);
        animator.SetBool("isStting", false);
    }

    private System.Collections.IEnumerator LeaveTimer()
    {
        while (GetTime() < 2f)
        {
            IncreaseTimer();
            yield return null;
        }

        ResetTimer();
        animator.SetBool("isChecking", false);
    }

    private void ResetTimer() => timer = 0f;
    private float GetTime() => timer;
    private void IncreaseTimer() => timer += Time.deltaTime;
}
