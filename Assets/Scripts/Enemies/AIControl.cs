using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    float startWaitTime = 2;
    float timeToRotate = 2;
    
    float moveSpeed = 8f;

    float viewRadius = 30f;
    float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    float meshResolution = 1f;
    int edgeIterations = 4;
    float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInChaseRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_PlayerInShootingRange;

    public TankWeapon equippedWeapon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_PlayerInShootingRange = false;
        m_PlayerInChaseRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

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
    }

    public void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;
       
       // if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
        //{
        //    m_PlayerInShootingRange = true;
        //}
        //else
        //{
          //  m_PlayerInShootingRange = false;
       // }

        if(!m_PlayerInShootingRange)
        {
            Move();
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        else{
            equippedWeapon.Fire();
        }
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if(m_WaitTime <= 0 && !m_PlayerInShootingRange && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                navMeshAgent.stoppingDistance = 15;
                m_PlayerNear = false;
                Move();
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            
            }
            else
            {
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
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
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
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

    void PlayerInShootingRange()
    {
        m_PlayerInShootingRange = true;
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
                    navMeshAgent.stoppingDistance = 30;
                }
                else{
                    m_PlayerInChaseRange = false; 
                }
            }
        }
        if (m_PlayerInChaseRange){
            m_PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }
}

