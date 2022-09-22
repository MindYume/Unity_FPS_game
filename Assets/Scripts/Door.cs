using UnityEngine;

public class Door : MonoBehaviour
{
    /// <summary> Moving part of the door </summary>
    [SerializeField] private GameObject movingPart;

    /// <summary> Is the door open </summary>
    [SerializeField] private bool _isOpen = false;

    /// <summary> Is the door activated. Activation allows to open the door </summary>
    private bool _isActivated = false;


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    void Update()
    {
        if (_isOpen)
        {
            movingPart.transform.localPosition = Vector3.MoveTowards(movingPart.transform.localPosition, Vector3.up, 3 * Time.deltaTime);
        }
        else
        {
            movingPart.transform.localPosition = Vector3.MoveTowards(movingPart.transform.localPosition, Vector3.zero, 3 * Time.deltaTime);
        }
    }

    /// <summary>
    /// OnTriggerStay is called once per physics update for every Collider other that is touching the trigger
    /// </summary>
    void OnTriggerStay(Collider other)
    {   
        switch(other.tag)
        {
            case "Player":
                _isOpen = _isActivated;
                break;
        }
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger
    /// </summary>
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            _isOpen = false;
        }
    }

    /// <summary>
    /// Returns true if the door is activated, otherwise false
    /// </summary>
    public bool IsActivated => _isActivated;

    /// <summary>
    /// Activates the door
    /// </summary>
    public void Activate()
    {
        _isActivated = true;
    }
}
