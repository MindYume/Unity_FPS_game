using UnityEngine;

public class MovementControl : MonoBehaviour
{  
    /// <summary> Player's RigidBody </summary>
    [SerializeField] private Rigidbody rb;

    /// <summary> Acceleration when the player moves </summary>
    [SerializeField] private float acceleration = 50f;

    /// <summary> Friction when the player is not controlled </summary>
    [SerializeField] private float friction = 50f;
    
    /// <summary> The speed the player will not exceed </summary>
    [SerializeField] private float standartVelocity = 5f;

    /// <summary> Mouse sensitivity </summary>
    [SerializeField] private float mouseSensitivity = 100f;


    /// <summary> Strength of jump </summary>
    [SerializeField] private float jumpStrength = 50f;

    /// <summary> Collider that checks if the player is on the ground </summary>
    private GroundCheck groundCheck;

    /// <summary> Time in which the player can gain vertical velocity</summary>
    private float _jumpTime;


    /// <summary> Player's camera </summary>
    private Camera cam;
    

    /// <summary> Camera direction </summary>
    private Vector2 _cameraLookDirection;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    /// </summary>
    void Start()
    {   
        groundCheck = GetComponentInChildren<GroundCheck>();
        cam = GetComponentInChildren<Camera>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    void Update()
    {
        HandleCameraControl();
    }

    /// <summary>
    /// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations
    /// </summary>
    void FixedUpdate()
    {
        HandleMovementControl();
        HandleJumpControl();
    }

    /// <summary>
    /// Changes the speed and direction of the objects's movement after pressing WASD or the arrow keys.
    /// This method should only be called inside the FixedUpdate method.
    /// </summary>
    private void HandleMovementControl()
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");
        moveDirection.Normalize();
        moveDirection = transform.right * moveDirection.x + transform.forward * moveDirection.z;

        // Horizontal velocity
        Vector2 hVelocity = new Vector2(rb.velocity.x, rb.velocity.z);

        if(moveDirection != Vector3.zero)
        {
            hVelocity.x += moveDirection.x * acceleration * Time.fixedDeltaTime;
            hVelocity.y += moveDirection.z * acceleration * Time.fixedDeltaTime;

            if(hVelocity.magnitude > standartVelocity)
                hVelocity = hVelocity.normalized*standartVelocity;

            rb.velocity = new Vector3(hVelocity.x, rb.velocity.y, hVelocity.y);
        }
        else
        {
            hVelocity = Vector2.MoveTowards(hVelocity, Vector2.zero, friction * Time.fixedDeltaTime);
        }

        rb.velocity = new Vector3(hVelocity.x, rb.velocity.y, hVelocity.y);
    }

    /// <summary>
    /// Changes the vertical speed of the player if the user pressed Space and the player is on the floor
    /// </summary>
    private void HandleJumpControl()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (rb.velocity.y <= 0 && groundCheck.IsOnFloor)
            {
                _jumpTime = 0.14f;
            }

            if (_jumpTime > 0)
            {
                rb.velocity = rb.velocity + Vector3.up * jumpStrength * Time.fixedDeltaTime;
            }
        }
        else
        {
            _jumpTime = 0f;
        }

        _jumpTime = _jumpTime > 0 ? _jumpTime - Time.fixedDeltaTime : 0;
    }
    
    /// <summary>
    /// Changes the direction of the camera when the mouse moves.
    /// This method should only be called inside the Update method.
    /// </summary>
    private void HandleCameraControl()
    {
        _cameraLookDirection.x -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        _cameraLookDirection.y += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        _cameraLookDirection.x = Mathf.Clamp(_cameraLookDirection.x, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(_cameraLookDirection.x, 0, 0);
        transform.rotation = Quaternion.Euler(0, _cameraLookDirection.y, 0);
    }
}   
