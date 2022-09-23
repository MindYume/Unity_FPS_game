using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPicker : MonoBehaviour
{
    public UnityEvent<Collider> triggerStay = new UnityEvent<Collider>();

    void OnTriggerStay(Collider other)
    {
        triggerStay.Invoke(other);
        print("test");
    }
}
