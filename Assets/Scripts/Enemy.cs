using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{   
    /// <summary> Bullet that enemy can spawn </summary>
    [SerializeField] private GameObject bullet;

    /// <summary> Speed of rotation </summary>
    [SerializeField] private float rotationSpeed = Mathf.PI;
    
    /// <summary> Waypoints between which the enemy will walk </summary>
    private Transform[] wayPoints;

    /// <summary> NavMeshAgent of the enemy </summary>
    private NavMeshAgent navMeshAgent;

    /// <summary> Position of the player </summary>
    private Vector3 _playerPosition;

    /// <summary> The number of the waypoint to which the enemy is currently following </summary>
    private int _wayPointNum = 0;

    /// <summary> Does the enemy see the player </summary>
    private bool _seePlayer = false;

    /// <summary> Delay before next attack </summary>
    private float _attackDelay = 1f;

    /// <summary> Is enemy ready to attack </summary>
    private bool _isReadyToAttack = true;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    /// </summary>
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        if ( wayPoints is not null && wayPoints.Length > 0)
        {
            navMeshAgent.destination = wayPoints[_wayPointNum].position;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    void Update()
    {
        if (_seePlayer)
        {
            TryFollowPlayer();
            TryArrackPlayer();
        }
        else
        {
            TryFollowWaypoints();
        }
    }

    /// <summary>
    /// OnTriggerStay is called once per physics update for every Collider other that is touching the trigger
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                _seePlayer = true;
                _playerPosition = other.gameObject.transform.position;
                break;
        }
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                _seePlayer = false;
                break;
        }
    }

    private void TryFollowPlayer()
    {
        navMeshAgent.angularSpeed = 0;

        if (Vector3.Distance(transform.position, _playerPosition) <= 2f)
        {
            navMeshAgent.destination = transform.position;
        }
        else
        {
            navMeshAgent.destination = _playerPosition;
        }
    }
    
    private void TryFollowWaypoints()
    {
        navMeshAgent.angularSpeed = 360;
        {
            if (Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination) < 0.1f)
            {
                if (_wayPointNum < wayPoints.Length-1)
                    _wayPointNum++;
                else
                    _wayPointNum = 0;

                navMeshAgent.destination = wayPoints[_wayPointNum].position;
            }
        }
    }

    private void TryArrackPlayer()
    {
        if (_seePlayer)
        {
           RotateTowardsPlayer();

           if (_isReadyToAttack)
           {
                SpawnBullet();
           }
        }
    }

    /// <summary>
    /// Sets a new path for the enemy
    /// </summary>
    /// <param name="wayPoints"> Waypoints between which the enemy will walk </param>
    public void SetPath(Transform[] wayPoints)
    {
        this.wayPoints = wayPoints;
    }

    /// <summary>
    /// Rotates enemy towards the player.
    /// This method should only be called inside the Update method.
    /// </summary>
    private void RotateTowardsPlayer()
    {
        Vector3 targetDirection = _playerPosition - transform.position;
        targetDirection.y = 0;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection.normalized, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    /// <summary>
    /// Prepares the enemy for fire
    /// </summary>
    private void SetReadyToAttack()
    {
        _isReadyToAttack = true;
    }

    /// <summary>
    /// Fires a bullet in the look direction
    /// </summary>
    private void SpawnBullet()
    {
        Bullet spawnedBullet = Instantiate(bullet, transform.position + transform.forward + (new Vector3(0, 1.5f, 0)), Quaternion.LookRotation(transform.forward)).GetComponent<Bullet>();
        spawnedBullet.SetTarget("Player");
        _isReadyToAttack = false;
        Invoke("SetReadyToAttack", _attackDelay);
    }
}
