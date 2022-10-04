using UnityEngine;
using UnityEngine.Events;

public class Player : Entity
{  
    [SerializeField] private GameObject bullet;
    
    /// <summary> Player's camera </summary>
    private Camera cam;

    /// <summary> Collider for collecting items </summary>
    private ObjectPicker objectPicker;
    
    /// <summary> Number of keys the player has </summary>
    private int _keys;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    /// </summary>
    void Start()
    {   
        cam = GetComponentInChildren<Camera>();
        objectPicker = GetComponentInChildren<ObjectPicker>();

        objectPicker.triggerStay.AddListener(OnObjectPickerTriggerEnter);

        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    void Update()
    {
        HandleAttackControl();
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

    /// <summary>
    /// Returns true if the player has at least one key, false otherwise
    /// </summary>
    public bool haveKey => _keys > 0;

}   
