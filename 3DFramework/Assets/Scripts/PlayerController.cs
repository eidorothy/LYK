using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _shootAction;

    private Vector2 _moveInput;

    private float _moveSpeed = 5f;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];

        _moveAction.performed += OnMove;    // 이동 입력 감지
        _moveAction.canceled += OnMoveStop; // 이동 중지 감지

        _attackAction = _playerInput.actions["Attack"];

        _attackAction.performed += OnAttack;

        _shootAction = _playerInput.actions["Shoot"];
        _shootAction.performed += OnShoot;
    }

    void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMoveStop;

        _attackAction.performed -= OnAttack;

        _shootAction.performed -= OnShoot;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnMoveStop(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        GameObject bullet = Managers.Game.Spawn(Define.ObjectType.Bullet, "Bullet");
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        int layerMask = LayerMask.GetMask("Wall", "Monster");       // wall이랑 monster에만 ray 적중

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        float maxDistance = 100.0f;     // 최대 사거리

        Vector3 endPoint;           // 끝점
        bool isHit = false;

        /* Ray 경로 위의 모든 오브젝트 감지
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
        foreach (RaycastHit h in hits)
        {
            if (h.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                endPoint = h.point;
                isHit = true;
                break;
            }
        }
        */

        //if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        if (Physics.Raycast(ray, out hit))
        {
            endPoint = hit.point;
            isHit = true;
        }
        else
        {
            endPoint = ray.origin + ray.direction * maxDistance;
        }

        Debug.DrawLine(ray.origin, endPoint, isHit ? Color.red : Color.yellow, 2.0f);
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
