using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{  

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private float mouseSensitivity = 2;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float friction = 20f;
    [SerializeField] private float maxVelocity = 5f;

    private Vector3 _moveDirection;
    

    void Update()
    {

        float yRotate = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float xRotate = -Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(new Vector3(0, yRotate, 0), Space.Self);
        cam.transform.Rotate(new Vector3(xRotate, 0, 0), Space.Self);
        cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, 0, 0);

        print(cam.transform.forward);

        _moveDirection = Vector3.zero;
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.z = Input.GetAxis("Vertical");
        
    }

    void FixedUpdate()
    {   
        float vY = rb.velocity.y;

        if (_moveDirection != Vector3.zero)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, _moveDirection * maxVelocity, acceleration * Time.fixedDeltaTime);
        }

        rb.velocity = new Vector3(rb.velocity.x, vY, rb.velocity.z);
    }
}
