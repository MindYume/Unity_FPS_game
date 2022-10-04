using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _deleteTime = 0.5f;
    private string _target = "All";
    private int _damage = 5;

    void Start()
    {   
        GetComponent<Rigidbody>().velocity = transform.forward * 15;

        Invoke("DeleteBullet", _deleteTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == _target)
        {
            other.GetComponentInParent<Entity>().TakeDamage(_damage);
            GameObject.Destroy(gameObject);
        }
    }

    public void SetTarget(string target)
    {
        _target = target;
    }

    private void DeleteBullet()
    {
        GameObject.Destroy(gameObject);
    }
}
