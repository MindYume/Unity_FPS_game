using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _deleteTime = 0.5f;

    void Start()
    {   
        GetComponent<Rigidbody>().velocity = transform.forward * 15;

        Invoke("DeleteBullet", _deleteTime);
    }

    private void DeleteBullet()
    {
        GameObject.Destroy(gameObject);
    }
}
