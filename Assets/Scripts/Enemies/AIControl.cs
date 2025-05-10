using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    float startWaitTime = 2;
    float timeToRotate = 2;
    
    float moveSpeed = 12f;

    float viewRadius = 30f;
    float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    float meshResolution = 1f;
    int edgeIterations = 4;
    float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    public Vector3 playerLastPosition = Vector3.zero;
    public Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    public bool m_PlayerInChaseRange;
    bool m_PlayerNear;
    public bool m_IsPatrol;
    bool m_PlayerInShootingRange;

    float patroling_distance = 0f;
    float chase_distance = 15f;

    float shootingDistance = 10f;

    public TankWeapon equippedWeapon;
    public Transform[] wheels;
    public float wheelRadius = 0.5f;

    private Vector3 previousPosition;

    public Transform turret;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_PlayerInShootingRange = false;
        m_PlayerInChaseRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;
        previousPosition = transform.position;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        EnviromentView();

        if(!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
        RotateWheels();
    }

    public void Chasing()
    {
        m_PlayerNear = false;       
       float distToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);

        if(m_PlayerInChaseRange)
        {
            RotateTurretTowardsPlayer();
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        else
        {
            navMeshAgent.SetDestination(playerLastPosition);
        }
        Move();

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            equippedWeapon.Fire();
            if(m_WaitTime <= 0 && !m_PlayerInShootingRange && distToPlayer >= 6f)
            {
                m_IsPatrol = true;
                navMeshAgent.stoppingDistance = patroling_distance;
                m_PlayerNear = false;
                Move();
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            
            }
        }
    }

    public void Patroling()
    {
        if(m_PlayerNear)
        {
            if(m_TimeToRotate <= 0)
            {
                Move();
                SearchingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if(m_WaitTime <= 0)
                {
                    NextPoint();
                    Move();
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
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
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void SearchingPlayer(Vector3 player){
        navMeshAgent.SetDestination(player);
        if(Vector3.Distance(transform.position, player) <= 0.3)
        {
            if(m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move();
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else{
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView(){
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for(int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle/2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if(!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInChaseRange = true;
                    m_IsPatrol = false;
                    navMeshAgent.stoppingDistance = chase_distance;
                }
                else{
                    m_PlayerInChaseRange = false; 
                }
            }
        }
        if (m_PlayerInChaseRange){
            playerLastPosition = m_PlayerPosition;
            m_PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    void RotateWheels()
    {
        // Calculate how far the tank has moved since the last frame
        Vector3 movement = transform.position - previousPosition;
        float distanceMoved = movement.magnitude;

        // Calculate rotation angle based on wheel circumference
        float rotationAngle = (distanceMoved / (2 * Mathf.PI * wheelRadius)) * 360f;

        // Apply rotation to each wheel
        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(Vector3.right, rotationAngle);
        }

        previousPosition = transform.position;
    }

    void RotateTurretTowardsPlayer()
{

    Vector3 direction = m_PlayerPosition - turret.position;
    direction.y = 0; // Ignore vertical difference to keep turret rotation level

    if (direction.sqrMagnitude > 0.01f)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

}


