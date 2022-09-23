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
            print(_isOnFloor);
        }
    }

    void OnTriggerExit()
    {
        _isOnFloor = false;
        print(_isOnFloor);
    }

    public bool IsOnFloor => _isOnFloor;
}
