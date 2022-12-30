using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    private GameObject[] patrolPoints;
    private GameObject[] celPoints;
    private int prevPatrolPoint = -1;
    private string State = "";
    private NavMeshAgent agent;
    private bool destinationReached = true;
    private bool celReached = false;
    private bool celChosen = false;
    private int celPoint = -1;
    private int chosenPoint = -1;
    public Animator animator;
    Vector3 prevPos;
    private Transform target;
    private Transform celLocation;
    private Transform notifyPoint;
    private bool targetInRange = false;
    private bool targetInAttackRange = false;
    private float attackCounter = 0f;
    private bool Attacked = false;
    private bool attackRoutine = false;
    private bool capturing = false;
    private string kindToCapture = "";

    public float startTime;
    private bool changed = false;
    [SerializeField] private GameObject knockedParticles;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        patrolPoints = GameObject.FindGameObjectsWithTag("Patrol");
        celPoints = GameObject.FindGameObjectsWithTag("Capture");
        prevPos = transform.position;
        startTime = -3f;
    }

    // Update is called once per frame
    private void Update()
    {
        DetectionModule();
        setState();
        if (State == "Capturing" && !celChosen)
        {
            ChooseCel();
        }
        if (State == "Attacking")
        {
            Attack();
        }
        if (State == "Following")
        {
            agent.SetDestination(target.position);
        }
        atDestination();
        atCel();
        if (State == "Notified")
        {
            agent.SetDestination(notifyPoint.position);
            Debug.Log("Notified");
        }
        atNotifiedPoint();
        if (State == "Patrolling" && destinationReached)
        {
            choosePatrolPoint();
        }

        // Stone
        if (Time.time-startTime < 3) {
            knockedParticles.SetActive(true);
            transform.position = new Vector3(prevPos.x, prevPos.y, 0);
            State = "None";
            changed = false;
        } else if (!changed) {
            changed = true;
            State = "Patrolling";
            knockedParticles.SetActive(false);
        }

    }

    private void setState()
    {
        if (capturing)
        {
            State = "Capturing";
            notifyPoint = null;
        }
        else if (targetInAttackRange || attackRoutine)
        {
            State = "Attacking";
            attackRoutine = true;
            notifyPoint = null;
        }
        else if (targetInRange)
        {
            State = "Following";
            destinationReached = true;
        }
        else if (notifyPoint != null)
        {
            State = "Notified";
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
            if (chosenPoint != prevPatrolPoint)
            {
                prevPatrolPoint = chosenPoint;
                pointChosen = true;
                destinationReached = false;
                agent.SetDestination(patrolPoints[chosenPoint].transform.position);
            }
        }
    }

    private void ChooseCel()
    {
        int bestCel = -1;
        float bestDistance = 10000;

        for (int i = 0; i < celPoints.Length; i++)
        {
            if (!celPoints[i].GetComponent<Occupied>().isOccupied())
            {
                float celDistance = Vector3.Distance(transform.position, celPoints[i].transform.position);
                if (celDistance < bestDistance)
                {
                    bestCel = i;
                    bestDistance = celDistance;
                }
            }
        }

        Debug.Log(bestCel);
        if (bestCel > -1)
        {
            celPoint = bestCel;
            celChosen = true;
            destinationReached = false;
            celPoints[bestCel].GetComponent<Occupied>().SetOccupied(true);
            celLocation = celPoints[bestCel].transform;
            agent.SetDestination(celPoints[bestCel].transform.position);
        }
    }

    private void atNotifiedPoint()
    {
        if(notifyPoint != null)
        {
            if (Mathf.Abs(transform.position.x - notifyPoint.position.x) < 0.005 && Mathf.Abs(transform.position.y - notifyPoint.position.y) < 0.005)
            {
                notifyPoint = null;
            }
        }
    }

    private void atDestination()
    {
        if (chosenPoint != -1)
        {
            if (Mathf.Abs(transform.position.x - patrolPoints[chosenPoint].transform.position.x) < 0.005 && Mathf.Abs(transform.position.y - patrolPoints[chosenPoint].transform.position.y) < 0.005)
            {
                destinationReached = true;
            }
        }
    }

    private void atCel()
    {
        if (celPoint != -1)
        {
            if (State == "Capturing")
            {
                if (Mathf.Abs(transform.position.x - celPoints[celPoint].transform.position.x) < 0.3 && Mathf.Abs(transform.position.y - celPoints[celPoint].transform.position.y) < 0.3)
                {
                    destinationReached = true;
                    letGo(false);
                }
            }
        }
    }

    private void DetectionModule()
    {
        if (Mathf.Abs(prevPos.x - transform.position.x) > 0.1 || Mathf.Abs(prevPos.y - transform.position.y) > 0.1)
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
        if (Mathf.Abs(prevPos.x - transform.position.x) < 0.001 && Mathf.Abs(prevPos.y - transform.position.y) > 0.001)
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

        if (attackCounter >= 3)
        {
            attackRoutine = false;
            attackCounter = 0f;
            Attacked = false;
        }
    }

    public void letGo(bool struggled)
    {
        capturing = false;
        kindToCapture = null;
        celLocation = null;
        celChosen = false;
        destinationReached = true;
        State = "Patrolling";

        if (struggled)
        {
            celPoints[celPoint].GetComponent<Occupied>().SetOccupied(false);
            celPoint = -1;
        }
        else
        {
            celPoint = -1;
        }
    }

    public void SetTarget(Transform collide)
    {
        target = collide;
    }

    public void setNotified(Transform notified)
    {
        notifyPoint = notified;
    }

    public void SetTargetInRange(bool inRange)
    {
        targetInRange = inRange;
    }

    public void SetTargetInAttackRange(bool inAttackRange)
    {
        targetInAttackRange = inAttackRange;

    }

    public void IsCapturing(bool capture)
    {
        capturing = capture;
    }

    public bool IsHolding()
    {
        return capturing;
    }

    public void kindtocapture(string kind)
    {
        kindToCapture = kind;
    }

    public Transform getCelLocation()
    {
        return celLocation;
    }
}

