using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : FSM
{

    #region Variables

    public Transform player;
    public Transform fovPos;
    public float maxAngle;
    public float maxRadius;

    private NavMeshAgent nav;

    public List<Transform> waypoints = new List<Transform>();
    public Transform targetWaypoint;
    public int targetWaypointIndex = 0;
    public int lastWaypointIndex = 3;

    public bool isInFov = false;
  
    public State currentState;

    public Animator animator;

    public GameObject checkingObj;

    public float waypointWaitTime;
    float nextWaypointTimer;

    public float returnHomeTime;
    float returnHomeTimer;

    public Vector3 lastSeenLocation;

    public bool isActive = false;

    public DeathCanvasScript deathCanvas;

    public GameManager gameManager;

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(fovPos.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, fovPos.up) * fovPos.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, fovPos.up) * fovPos.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(fovPos.position, fovLine1);
        Gizmos.DrawRay(fovPos.position, fovLine2);
        if (!isInFov)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawRay(fovPos.position, (player.position - fovPos.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(fovPos.position, fovPos.forward * maxRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        nextWaypointTimer = waypointWaitTime;

        targetWaypoint = waypoints[targetWaypointIndex];
        nav.destination = targetWaypoint.position;

        currentState = new EnemyPatrolState(this);
        this.SetState(currentState);

        if (!isActive)
        {
            animator.SetBool("walking", false);
            animator.SetBool("idle", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isInFov = InFOV(fovPos, player, maxAngle, maxRadius);
        GetComponent<Rigidbody>().velocity = Vector3.zero;


        if (isActive)
        {
            this.GetState().action();
        }
        else
            if (Vector3.Distance(player.position, transform.position) <= 1.5f && isInFov)
                isActive = true;
    }

    public void moveToNextWaypoint()
    {
        if (targetWaypointIndex == lastWaypointIndex)
        {
            targetWaypointIndex = 0;
        }
        else
        {
            targetWaypointIndex++;
        }

        targetWaypoint = waypoints[targetWaypointIndex];
        nav.destination = targetWaypoint.position;
    }


    public void returnToLastWaypoint()
    {
        animator.SetBool("shrug", false);
        animator.SetBool("walking", true);
        nav.destination = targetWaypoint.position;

        currentState = new EnemyPatrolState(this);
        this.SetState(currentState);
    }

    public void MoveToHideable(Transform targetWaypoint)
    {
        nav.destination = targetWaypoint.position;
    }

    public void PatrolState()
    {
        if (!isInFov)
        {
            float distance = Mathf.Floor(Vector3.Distance(transform.position, targetWaypoint.position));

            if (distance <= nav.stoppingDistance)
            {
                nav.speed = 0;
                nextWaypointTimer -= Time.deltaTime;
                animator.SetBool("walking", false);
                animator.SetBool("idle", true);              

                if (nextWaypointTimer <= 0)
                {
                    animator.SetBool("walking", true);
                    animator.SetBool("idle", false);

                    nextWaypointTimer = waypointWaitTime;
                    moveToNextWaypoint();
                }
            }
            else
            {
                nav.speed = 1.0f;
                animator.SetBool("walking", true);
                animator.SetBool("idle", false);
            }
        }
        else if (isInFov)
        {
            nextWaypointTimer = waypointWaitTime;              
            animator.SetBool("walking", false);
            animator.SetBool("idle", false);
            animator.SetBool("chase", true);

            nav.destination = player.position;

            currentState = new EnemyChaseState(this);
            this.SetState(currentState);
        }
    }

    public void ChasingState()
    {
        nav.speed = 1.5f;

        if (!isInFov)
        {
            nav.destination = lastSeenLocation;
            float dist = Vector3.Distance(transform.position, lastSeenLocation);
            Debug.Log(dist);


            if (dist <= nav.stoppingDistance)
            {
                nav.speed = 1;

                bool foundObj = false;
                Collider[] objs = Physics.OverlapSphere(fovPos.position, 3);
                foreach (Collider obj in objs)
                {
                    if (obj.gameObject.tag == "Hideable")
                    {
                        foundObj = true;
                        checkingObj = obj.gameObject;
                        break;
                    }
                }

                if (foundObj)
                {
                    returnHomeTimer = returnHomeTime;

                    nav.destination = checkingObj.GetComponent<hideableObject>().exitSpot.transform.position;

                    currentState = new EnemyCheckingState(this);
                    this.SetState(currentState);
                }
                else
                {
                    nav.speed = 0f;
                    animator.SetBool("chase", false);
                    animator.SetBool("looking", true);

                    returnHomeTimer -= Time.deltaTime;

                    if (returnHomeTimer <= 2f)
                    {
                        animator.SetBool("chase", false);
                        animator.SetBool("looking", false);
                        animator.SetBool("shrug", true);
                    }

                    if (returnHomeTimer <= 0)
                    {                       
                        returnToLastWaypoint();
                    }
                }
            }
        }

        else if (isInFov)
        {
            if(Vector3.Distance(transform.position, player.position) <= 1.5f)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                gameManager.gameState = GameState.Menu;

                player.gameObject.GetComponent<PlayerControllerTest>().currentState = new PlayerDeadState(player.gameObject.GetComponent<PlayerControllerTest>());
                player.gameObject.GetComponent<PlayerControllerTest>().SetState(player.gameObject.GetComponent<PlayerControllerTest>().currentState);

                player.gameObject.GetComponent<Animator>().SetBool("walking", false);
                player.gameObject.GetComponent<Animator>().SetBool("crouching", false);

                currentState = new EnemyFoundPlayerState(this);
                this.SetState(currentState);

                deathCanvas.TurnOn();
            }


            animator.SetBool("chase", true);
            animator.SetBool("looking", false);
            animator.SetBool("shrug", false);


            returnHomeTimer = returnHomeTime;
            nav.destination = player.position;
        }
    }

    public void CheckingState()
    {
        if (!isInFov)
        {
            float distToHideable = /*Mathf.Floor(*/Vector3.Distance(transform.position, checkingObj.GetComponent<hideableObject>().exitSpot.transform.position);//);
            Debug.Log(distToHideable);

            if (distToHideable <= nav.stoppingDistance)
            {
                nav.speed = 0f;
                transform.LookAt(new Vector3(checkingObj.transform.position.x, this.transform.position.y, checkingObj.transform.position.z));

                // play animation for checking hideable
                animator.SetBool("chase", false);
                animator.SetBool("walking", false);

                //put idle animation here for now
                animator.SetBool("idle", true);

                // if player is found then player loses
                bool foundObj = false;
                Collider[] objs = Physics.OverlapSphere(fovPos.position + Vector3.forward, 2);
                foreach (Collider obj in objs)
                {
                    if (obj.gameObject.tag == "player")
                    {
                        foundObj = true;
                        break;
                    }
                }

                if (foundObj)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    gameManager.gameState = GameState.Menu;

                    player.gameObject.GetComponent<PlayerControllerTest>().currentState = new PlayerDeadState(player.gameObject.GetComponent<PlayerControllerTest>());
                    player.gameObject.GetComponent<PlayerControllerTest>().SetState(player.gameObject.GetComponent<PlayerControllerTest>().currentState);

                    currentState = new EnemyFoundPlayerState(this);
                    this.SetState(currentState);

                    deathCanvas.TurnOn();                    
                }

                //else play some looking animations and go back to patrolling;
                else
                {
                    animator.SetBool("chase", false);
                    animator.SetBool("idle", false);
                    animator.SetBool("looking", true);

                    returnHomeTimer -= Time.deltaTime;

                    if (returnHomeTimer <= 2f)
                    {
                        animator.SetBool("chase", false);
                        animator.SetBool("looking", false);
                        animator.SetBool("shrug", true);
                    }

                    if (returnHomeTimer <= 0)
                    {
                        animator.SetBool("shrug", false);
                        animator.SetBool("walking", true);

                        returnHomeTimer = returnHomeTime;
                        checkingObj = null;

                        returnToLastWaypoint();
                    }
                }
            }
            else if (distToHideable > nav.stoppingDistance)
            {
                nav.destination = checkingObj.GetComponent<hideableObject>().exitSpot.transform.position;
            }
        }
        else if (isInFov)
        {
            animator.SetBool("chase", true);
            animator.SetBool("looking", false);
            animator.SetBool("shrug", false);

            nav.destination = player.position;
            returnHomeTimer = returnHomeTime;

            checkingObj = null;

            currentState = new EnemyChaseState(this);
            this.SetState(currentState);
        }
    }

    public void FoundPlayer()
    {
        animator.SetBool("chase", false);
        animator.SetBool("looking", false);
        animator.SetBool("shrug", false);
        animator.SetBool("walking", false);
        animator.SetBool("idle", true);

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        nav.speed = 0;
    }

    public bool InFOV(Transform checkingObj, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10000];
        int count = Physics.OverlapSphereNonAlloc(checkingObj.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == target)
                {
                    Vector3 directionBetween = (target.position - checkingObj.position).normalized;
                    //directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObj.forward, directionBetween);

                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObj.position, target.position - checkingObj.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            if (hit.transform == target)
                            {
                                lastSeenLocation = target.transform.position;

                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

}
