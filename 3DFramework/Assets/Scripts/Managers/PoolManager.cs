using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();    //"Bullet", 생성된 Bullet 바구니
    Transform _root;

    public PoolManager Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }

        return this;
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        if (original == null)
            return;

        if (_pool.ContainsKey(original.name))
            return;

        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.SetParent(_root);

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)   // 반납
    {
        string name = poolable.gameObject.name;

        if (_pool.ContainsKey(name) == false)
        {
            Object.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)   // 가져가기
    {
        if (_pool.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    public void Clear()
    {
        foreach(var pool in _pool.Values)
        {
            Object.Destroy(pool.Root.gameObject);
        }

        _pool.Clear();
    }
}
