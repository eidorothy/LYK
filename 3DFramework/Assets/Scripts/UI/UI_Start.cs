using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Start : MonoBehaviour
{
    Button _button;
    public Define.Scene _sceneType;

    void Start()
    {
        _button = GetComponent<Button>();
        _button?.onClick.AddListener(ChangeScene);  // 구독
    }

    void ChangeScene()
    {
        Managers.Scene.LoadScene(_sceneType);
    }
}
