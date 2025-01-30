using System;
using UnityEngine;

public class InputManager
{
    public Action<KeyCode> KeyAction = null;
    

    public void OnUpdate()
    {
        if (Input.anyKey && KeyAction != null)
        {
            // Unity의 GetKey를 각각 클래스의 Update에서 하지 않고 이쪽에서 처리해 준다
            KeyCode keyCode = KeyCode.None;
            if (Input.inputString.Length > 0)
                keyCode = (KeyCode)Input.inputString[0];
            KeyAction.Invoke(keyCode);  // W
            // 키가 눌렸어 -> Action에 구독하고 있는 Player OnKeyInput을 실행
        }

        // TODO : 마우스 입력 / 화면 터치 입력
    }

    public void Clear()
    {
        KeyAction = null;
    }
}
