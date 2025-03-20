using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CustomerAnim : MonoBehaviour
{
    private CustomerScript customer;

    private Transform targetLoc;
    public Transform originLoc;
    bool arrived = false;

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
        if (!arrived && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            { 
                if (agent.pathEndPosition == targetLoc.position)
                {
                    OnArrive();
                    arrived = true;
                }
            }
        }
    }

    public void MoveToLocation(Transform loc)
    {
        targetLoc = loc;
        agent.SetDestination(loc.position);
        animator.SetBool("isMove", true);
        arrived = false;
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
        targetLoc = originLoc;

        agent.SetDestination(originLoc.transform.position);

        animator.SetBool("isStting", false);
        animator.SetBool("isMove", true);
        arrived = false;
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
