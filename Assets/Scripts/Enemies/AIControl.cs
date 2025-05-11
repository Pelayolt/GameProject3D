using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public TankWeapon equippedWeapon;
    public Transform[] wheels;
    public Transform turret;
    private Transform playerTransform;


    public float moveSpeed = 12f;
    public float viewRadius = 30f;
    public float viewAngle = 90f;
    public float shootingDistance = 10f;
    public float wheelRadius = 0.5f;
    public float patrolingDistance = 0f;
    public float chaseDistance = 15f;

    private int currentWaypointIndex = 0;
    private Vector3 previousPosition;
    public Vector3 playerLastPosition = Vector3.zero;
    public Vector3 playerPosition = Vector3.zero;

    private float startWaitTime = 2f;
    private float timeToRotate = 2f;
    private float waitTime;
    private float rotateTime;

    public bool isPatrolling = true;
    public bool playerInChaseRange = false;
    private bool playerNear = false;
    private bool playerInShootingRange = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

        previousPosition = transform.position;
        waitTime = startWaitTime;
        rotateTime = timeToRotate;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        EnviromentView();

        if (!isPatrolling)
        {
            playerPosition = playerTransform.position;
            Chasing();
        }
        else
        {
            Patrolling();
        }
        RotateWheels();
    }


    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        foreach (var hit in playerInRange)
        {
            Transform player = hit.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    playerInChaseRange = true;
                    isPatrolling = false;
                    navMeshAgent.stoppingDistance = chaseDistance;

                    playerLastPosition = playerPosition;
                    playerPosition = player.position;
                    return;
                }
            }
        }
        playerInChaseRange = false;
    }

    void Chasing()
    {
        playerNear = true;
        float distToPlayer = Vector3.Distance(transform.position, playerPosition);
        playerInShootingRange = distToPlayer <= shootingDistance;

        navMeshAgent.SetDestination(playerInChaseRange ? playerPosition : playerLastPosition);
        Move();

        if (!(equippedWeapon is PlasmaBeam pb && pb.isFiring))
        {
            RotateTurretTowardsPlayer();
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (equippedWeapon is Flamethrower flamethrower)
            {
                if (playerInShootingRange) flamethrower.Fire();
                else flamethrower.StopFire();
            }
            else if (playerInShootingRange)
            {
                equippedWeapon.Fire();
            }

            if (waitTime <= 0 && !playerInShootingRange && distToPlayer >= 6f)
            {
                isPatrolling = true;
                navMeshAgent.stoppingDistance = patrolingDistance;
                Move();
                rotateTime = timeToRotate;
                waitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

            }
        }
        else if (equippedWeapon is Flamethrower flamethrower3)
        {
            flamethrower3.StopFire();
        }
    }

    void Patrolling()
    {
        if (playerNear)
        {
            if (rotateTime <= 0)
            {
                Move();
                SearchingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                rotateTime -= Time.deltaTime;
            }
        }
        else
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (waitTime <= 0)
                {
                    NextPoint();
                    Move();
                    waitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }

    void SearchingPlayer(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
        if (Vector3.Distance(transform.position, target) <= 0.3f)
        {
            if (waitTime <= 0)
            {
                playerNear = false;
                Move();
                navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
                waitTime = startWaitTime;
                rotateTime = timeToRotate;
            }
            else
            {
                Stop();
                waitTime -= Time.deltaTime;
            }
        }
    }

    void Move()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = moveSpeed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0f;
    }

    void NextPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void RotateWheels()
    {
        Vector3 movement = transform.position - previousPosition;
        float distanceMoved = movement.magnitude;
        float rotationAngle = (distanceMoved / (2 * Mathf.PI * wheelRadius)) * 360f;

        foreach (Transform wheel in wheels)
            wheel.Rotate(Vector3.right, rotationAngle);

        previousPosition = transform.position;
    }

    void RotateTurretTowardsPlayer()
    {
        Vector3 direction = playerPosition - turret.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, Time.deltaTime * 2f);
        }
    }


}
