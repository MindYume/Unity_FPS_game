using UnityEngine;

public class FireEffect : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
    }
}
