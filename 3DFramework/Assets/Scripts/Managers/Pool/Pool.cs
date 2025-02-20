using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public GameObject Original { get; private set; }
    public Transform Root { get; set; }

    Stack<Poolable> _poolStack;

    public void Init(GameObject original, int count = 5)
    {
        Original = original;
        Root = new GameObject($"{Original.name}_Root").transform;   // Bullet_Root

        _poolStack = new Stack<Poolable>();

        // for문 돌면서 총 5개 생성 작업
        for (int i = 0; i < count; ++i)
        {
            Push(Create());
        }
    }

    Poolable Create()
    {
        //GameObject go = Managers.Resource.Instantiate()
        GameObject go = Object.Instantiate(Original);
        go.name = Original.name;
        return go.GetOrAddComponent<Poolable>();
    }

    public void Push(Poolable poolable)
    {
        if (poolable == null)
            return;

        poolable.OnPush();
        poolable.isUsing = false;
        poolable.transform.SetParent(Root);

        _poolStack.Push(poolable);
    }

    public Poolable Pop(Transform parent)
    {
        Poolable poolable = (_poolStack.Count > 0) ? _poolStack.Pop() : Create();

        poolable.OnPop();
        poolable.isUsing = true;
        poolable.transform.SetParent(parent ?? Managers.Scene.CurrentScene.transform);

        return poolable;
    }
}
