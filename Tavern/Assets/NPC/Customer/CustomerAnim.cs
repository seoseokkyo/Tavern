using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CustomerAnim : MonoBehaviour
{
    private CustomerScript customer;

    private Transform targetLoc;
    public Transform originLoc;
    bool arrived = false;
    public bool isMoving = false;

    private NavMeshAgent agent;
    public Animator animator;

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

            customer.Initialize();
            customer.DecideOrder();
        }
    }

    public void Leave()
    {
        animator.SetBool("isStting", false);
        targetLoc = originLoc;
        agent.SetDestination(originLoc.position);
        animator.SetBool("isMove", true);

        arrived = false;
        isMoving = true;
    }

    public void Check(bool correct)
    {
        animator.SetBool("isChecking", true);
        if(correct)
        {
            animator.SetBool("isCorrect", true);
        }
        else
        {
            animator.SetBool("isCorrect", false);
        }
    }

    public void Eat()
    {
        animator.SetBool("isChecking", false);
    }
}
