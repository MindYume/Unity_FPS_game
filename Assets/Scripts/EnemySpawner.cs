using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform[] wayPoints;

    void Start()
    {
        GameObject createdEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        createdEnemy.GetComponent<Enemy>().SetPath(wayPoints);
    }

}
