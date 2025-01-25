using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    // Rotation Settings
    [SerializeField] private float _lookSpeed = 2f;
    [SerializeField] private float _lookXTopLimit = 55f;
    [SerializeField] private float _lookXBotLimit = 55f;
    float rotationX = 0f;
    
    // Movement Settings
    [SerializeField] private float _gravityScale = 9.81f;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private float _walkSpeed = 3f;

    CharacterController characterController;
    [SerializeField] private Camera _playerCamera;
    private bool _isWalking = false;
    
    public bool IsWalking { get { return _isWalking; } }
    
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement
        Vector3 forward = transform.forward;
    	Vector3 moveDirection = new Vector3(_walkSpeed * Input.GetAxis("Horizontal"), -_gravityScale, _walkSpeed * Input.GetAxis("Vertical"));
        
        // Handle rotation
        if (_canMove)
        {
            characterController.Move(transform.rotation * moveDirection * Time.deltaTime);
            
            rotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -_lookXBotLimit, _lookXTopLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X")* _lookSpeed, 0);
        }
        
		_isWalking = !(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0);
        
        
    }
}