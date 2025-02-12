using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private Vector2 _moveInput;

    private float _moveSpeed = 5f;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];

        _moveAction.performed += OnMove;    // 이동 입력 감지
        _moveAction.canceled += OnMoveStop; // 이동 중지 감지
    }

    void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMoveStop;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnMoveStop(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    void Update()
    {
        Vector3 dir = new Vector3(_moveInput.x, 0, _moveInput.y);
        transform.position += dir * _moveSpeed * Time.deltaTime;
        
        /*
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * _moveSpeed * Time.deltaTime;
            Debug.Log("앞으로 이동");
        }*/
    }
}
