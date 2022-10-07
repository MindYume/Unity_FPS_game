using UnityEngine;

public class FireEffect : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().time = Random.Range(0f, GetComponent<AudioSource>().clip.length);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
    }
}
