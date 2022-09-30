using UnityEngine;

public class Player : MonoBehaviour
{  
    [SerializeField] private GameObject bullet;

    /// <summary> Mouse sensitivity </summary>
    [SerializeField] private float mouseSensitivity = 100f;

    /// <summary> Acceleration when the player moves </summary>
    [SerializeField] private float acceleration = 50f;

    /// <summary> Friction when the player is not controlled </summary>
    [SerializeField] private float friction = 50f;
    
    /// <summary> The speed the player will not exceed </summary>
    [SerializeField] private float standartVelocity = 5f;

    /// <summary> Strength of jump </summary>
    [SerializeField] private float jumpStrength = 50f;
    
    /// <summary> Player's camera </summary>
    private Camera cam;
    
    /// <summary> Player's RigidBody </summary>
    private Rigidbody rb;

    /// <summary> Collider that checks if the player is on the ground </summary>
    private GroundCheck groundCheck;

    /// <summary> Collider for collecting items </summary>
    private ObjectPicker objectPicker;

    /// <summary> Camera direction </summary>
    private Vector2 _cameraLookDirection;

    /// <summary> Time in which the player can gain vertical velocity</summary>
    private float _jumpTime;
    
    /// <summary> Number of keys the player has </summary>
    private int _keys;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    /// </summary>
    void Start()
    {   
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        objectPicker = GetComponentInChildren<ObjectPicker>();

        objectPicker.triggerStay.AddListener(OnObjectPickerTriggerEnter);

        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    void Update()
    {
        HandleCameraControl();
        HandleMovementControl();
        HandleJumpControl();
        HandleAttackControl();
    }

    /// <summary>
    /// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations
    /// </summary>
    void FixedUpdate()
    {   
        // HandleMovementControl();
    }

    /// <summary>
    /// When a GameObject collides with another GameObject, Unity calls OnTriggerEnter
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Door":
                TryActivateDoor(other.GetComponent<Door>());
                break;
        }
    }

    /// <summary>
    /// Collider for collecting items
    /// </summary>
    private void OnObjectPickerTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Key":
                PickKey(other.GetComponent<Key>());
                break;
        }
    }

    /// <summary>
    /// Returns true if the player has at least one key, false otherwise
    /// </summary>
    public bool haveKey => _keys > 0;

    /// <summary>
    /// Changes the speed and direction of the player's movement after pressing WASD or the arrow keys.
    /// This method should only be called inside the Update method.
    /// </summary>
    private void HandleMovementControl()
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");
        moveDirection.Normalize();
        moveDirection = transform.right * moveDirection.x + transform.forward * moveDirection.z;

        // Горизонтальная скорость
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
                rb.velocity = rb.velocity + Vector3.up * jumpStrength * Time.deltaTime;
            }
        }
        else
        {
            _jumpTime = 0f;
        }

        _jumpTime = _jumpTime > 0 ? _jumpTime - Time.deltaTime : 0;
    }

    /// <summary>
    /// Spawns a bullet in the direction the player is looking if the user clicks the left mouse button.
    /// </summary>
    private void HandleAttackControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, cam.transform.position + transform.forward, Quaternion.LookRotation(cam.transform.forward));
        }
    }

    /// <summary>
    /// Picks up a key. After picking up the key, object is removed, and the number of keys the player has increases
    /// </summary>
    private void PickKey(Key key)
    {
        GameObject.Destroy(key.gameObject);
        _keys++;
    }

    /// <summary>
    /// Activates the door if the player has at least one key, after which the number of keys the player has is reduced by one
    /// </summary>
    private void TryActivateDoor(Door door)
    {   if (!door.IsActivated && _keys > 0)
        {
            door.Activate();
            _keys--;
        }
    }
    
}   
