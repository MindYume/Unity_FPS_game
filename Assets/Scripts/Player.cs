using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{  

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float friction = 50f;
    [SerializeField] private float maxVelocity = 5f;

    private Vector2 _moveDirection;
    private float xCameraRotation;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        float xMouseRotate = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float yMouseRotate = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xCameraRotation -= yMouseRotate;
        xCameraRotation = Mathf.Clamp(xCameraRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xCameraRotation, 0, 0);
        transform.Rotate(new Vector3(0, xMouseRotate, 0));


        /* transform.Rotate(new Vector3(0, yRotate, 0), Space.Self);
        cam.transform.Rotate(new Vector3(xRotate, 0, 0), Space.Self);
        cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, 0, 0);

        print(cam.transform.forward);

        _moveDirection = Vector3.zero;
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.z = Input.GetAxis("Vertical"); */
        
    }

    void FixedUpdate()
    {   
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");
        moveDirection.Normalize();
        moveDirection = transform.right * moveDirection.x + transform.forward * moveDirection.z;

        if(moveDirection != Vector3.zero)
        {
            Vector2 hVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
            hVelocity.x += moveDirection.x * acceleration * Time.fixedDeltaTime;
            hVelocity.y += moveDirection.z * acceleration * Time.fixedDeltaTime;

            if(hVelocity.magnitude > maxVelocity)
                hVelocity = hVelocity.normalized*maxVelocity;

            rb.velocity = new Vector3(hVelocity.x, rb.velocity.y, hVelocity.y);
        }
        else
        {
            Vector2 hVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
            hVelocity = Vector2.MoveTowards(hVelocity, Vector2.zero, friction * Time.fixedDeltaTime);
            rb.velocity = new Vector3(hVelocity.x, rb.velocity.y, hVelocity.y);
        }


        /* float vY = rb.velocity.y;

        if (_moveDirection != Vector3.zero)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, _moveDirection * maxVelocity, acceleration * Time.fixedDeltaTime);
        }

        rb.velocity = new Vector3(rb.velocity.x, vY, rb.velocity.z); */
    }
}
