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
    private float _horizontalLookDirection;
    private float _xCameraRotation;
    private int _keys = 0;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        float xMouseRotate = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float yMouseRotate = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xCameraRotation -= yMouseRotate;
        _xCameraRotation = Mathf.Clamp(_xCameraRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(_xCameraRotation, 0, 0);
        _horizontalLookDirection += xMouseRotate;

        transform.rotation = Quaternion.Euler(0,_horizontalLookDirection,0);
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
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Key")
        {
            GameObject.Destroy(collider.gameObject);
            _keys++;
        }
    }

    public bool haveKey => _keys > 0 ? true : false;

    public void ConsumeKey()
    {
        if (_keys > 0) 
            _keys--;
    }
}
