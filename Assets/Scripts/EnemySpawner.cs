using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject enemy;

    void Start()
    {
        Instantiate(enemy, spawnPoint.position + new Vector3(0, 1, 0), Quaternion.identity);
    }

}
