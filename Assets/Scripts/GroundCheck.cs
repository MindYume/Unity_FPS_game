using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool _isOnFloor = false;

    void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger)
        {
            _isOnFloor = true;
        }
    }

    void OnTriggerExit()
    {
        _isOnFloor = false;
    }

    public bool IsOnFloor => _isOnFloor;
}
