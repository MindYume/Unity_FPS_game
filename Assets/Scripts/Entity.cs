using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{  
    public UnityEvent<int> onHealthChange = new UnityEvent<int>();

    private int _health = 100;

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public virtual int Health
    {
        get => _health;
        set
        {
            _health = value;
            onHealthChange.Invoke(_health);
        }
    }
    
}   
