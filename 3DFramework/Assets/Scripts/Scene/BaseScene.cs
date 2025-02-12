using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.None;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Debug.Log("Init BaseScene");
        
        // 씬 전체적으로 처리되어야 할 Init
    }

    public virtual void Clear()
    {

    }
}
