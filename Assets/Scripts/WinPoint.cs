using UnityEngine;
using UnityEngine.Events;


public class WinPoint : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerEnter = new UnityEvent();

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onTriggerEnter.Invoke();
            SceneLoader.Load("Level_2");
        }
    }
}
