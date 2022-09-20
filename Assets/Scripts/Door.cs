using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject movingPart;
    [SerializeField] private bool _isOpen = false;
    private bool _isActivated = false;

    void Update()
    {
        print(movingPart.transform.localPosition);
        if (_isOpen)
        {
            movingPart.transform.localPosition = Vector3.MoveTowards(movingPart.transform.localPosition, Vector3.up * 4.5f, 3 * Time.deltaTime);
        }
        else
        {
            movingPart.transform.localPosition = Vector3.MoveTowards(movingPart.transform.localPosition, Vector3.up * 1.5f, 3 * Time.deltaTime);
        }
    }


    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (_isActivated)
            {
                _isOpen = true;
            }
            else if(collider.GetComponentInParent<Player>().haveKey)
            {
                _isActivated = true;
                collider.GetComponentInParent<Player>().ConsumeKey();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            _isOpen = false;
        }
    }
}
