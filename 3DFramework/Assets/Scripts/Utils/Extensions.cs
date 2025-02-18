using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    // 게임 오브젝트가 실제로 있고, active 상태인지를 체크하고 싶음
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }

        return component;
    }
}
