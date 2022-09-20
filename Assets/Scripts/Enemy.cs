using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    /// <summary> Bullet that enemy can spawn </summary>
    [SerializeField] private GameObject bullet;

    /// <summary> Speed of rotation </summary>
    [SerializeField] private float rotationSpeed = Mathf.PI/2;

    /// <summary> Does the enemy see the player </summary>
    private bool _seePlayer = false;
    
    /// <summary> Position of the player </summary>
    private Vector3 _playerPosition;

    /// <summary> Delay before next attack </summary>
    private float _attackDelay = 1f;

        /// <summary> Is enemy ready to attack </summary>
    private bool _isReadyToAttack = true;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    void Update()
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
        Instantiate(bullet, transform.position + transform.forward + (new Vector3(0, 0.5f, 0)), Quaternion.LookRotation(transform.forward));
        _isReadyToAttack = false;
        Invoke("SetReadyToAttack", _attackDelay);
    }

}
