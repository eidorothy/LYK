using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _moveSpeed = 5f;

    void OnEnable()
    {
        // 안전하게 해제 후 등록 => 중복 등록 방지
        Managers.Input.KeyAction -= OnKeyInput;     // 키 이벤트 구독 해제
        Managers.Input.KeyAction += OnKeyInput;     // 키 이벤트 구독
    }

/*
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * _moveSpeed * Time.deltaTime;
            Debug.Log("앞으로 이동");
        }
    }
*/

    void OnKeyInput(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                transform.position += Vector3.forward * _moveSpeed * Time.deltaTime;
                Debug.Log("앞으로 이동");
                break;
        }
    }
}
