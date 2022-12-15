using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    private GameObject[] patrolPoints;
    private int prevPatrolPoint = -1;
    private string State = "";
    private NavMeshAgent agent;
    private bool destinationReached = true;
    private int chosenPoint = -1;
    public Animator animator;
    Vector3 prevPos;
    private Transform target;
    private bool targetInRange = false;
    private bool targetInAttackRange = false;
    private float attackCounter = 0f;
    private bool Attacked = false;
    private bool attackRoutine = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        patrolPoints = GameObject.FindGameObjectsWithTag("Patrol");
        prevPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        DetectionModule();
        setState();
        if(State == "Attacking")
        {
            Attack();
        }
        if (State == "Following")
        {
            agent.SetDestination(target.position);
        }
        atDestination();
        if(State == "Patrolling" && destinationReached)
        {
            choosePatrolPoint();
        }
    }

    private void setState()
    {
        if (targetInAttackRange || attackRoutine)
        {
            State = "Attacking";
            attackRoutine = true;
        }
        else if (targetInRange)
        {
            State = "Following";
            destinationReached = true;
        }
        else
        {
            State = "Patrolling";
        }
    }

    private void choosePatrolPoint()
    {
        bool pointChosen = false;

        while (!pointChosen)
        {
            chosenPoint = Random.Range(0, patrolPoints.Length);
            if(chosenPoint != prevPatrolPoint)
            {
                prevPatrolPoint = chosenPoint;
                pointChosen = true;
                destinationReached = false;
                agent.SetDestination(patrolPoints[chosenPoint].transform.position);
            }
        }
    }

    private void atDestination()
    {
        if(chosenPoint != -1)
        {
            if (Mathf.Abs(transform.position.x - patrolPoints[chosenPoint].transform.position.x) < 0.005 && Mathf.Abs(transform.position.y - patrolPoints[chosenPoint].transform.position.y) < 0.005)
            {
                destinationReached = true;
            }
        }
    }

    private void DetectionModule()
    {
        if(Mathf.Abs(prevPos.x - transform.position.x) > 0.1 || Mathf.Abs(prevPos.y - transform.position.y) > 0.1)
        {
            Vector3 aimDirection = (this.transform.position - prevPos).normalized;
            Vector2 rayDirection = new Vector2(aimDirection.x, aimDirection.y);

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            if (angle > 45 && angle < 135)
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", 1);
            }
            if (angle < -135 || angle > 135)
            {
                animator.SetFloat("Horizontal", -1);
                animator.SetFloat("Vertical", 0);
            }
            if (angle > -45 && angle < 45)
            {
                animator.SetFloat("Horizontal", 1);
                animator.SetFloat("Vertical", 0);
            }
            if (angle > -135 && angle < -45)
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", -1);
            }

            animator.SetFloat("Speed", 1);
            prevPos = transform.position;
        }
        if(Mathf.Abs(prevPos.x - transform.position.x) < 0.001 && Mathf.Abs(prevPos.y - transform.position.y) > 0.001)
        {
            animator.SetFloat("Speed", 0);
        }
    }

    private void Attack()
    {
        if (!Attacked)
        {
            Vector3 aimDirection = (target.position - this.transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            if (angle > 45 && angle < 135)
            {
                animator.SetTrigger("AttackUp");
            }
            if (angle < -135 || angle > 135)
            {
                animator.SetTrigger("AttackLeft");
            }
            if (angle > -45 && angle < 45)
            {
                animator.SetTrigger("AttackRight");
            }
            if (angle > -135 && angle < -45)
            {
                animator.SetTrigger("AttackDown");
            }
            Attacked = true;
        }
        attackCounter += Time.deltaTime;

        if(attackCounter >= 3)
        {
            attackRoutine = false;
            attackCounter = 0f;
            Attacked = false;
        }
    }

    public void SetTarget(Transform collide)
    {
        target = collide;
    }

    public void SetTargetInRange(bool inRange)
    {
        targetInRange = inRange;
    }

    public void SetTargetInAttackRange(bool inAttackRange)
    {
        targetInAttackRange = inAttackRange;

    }
}
