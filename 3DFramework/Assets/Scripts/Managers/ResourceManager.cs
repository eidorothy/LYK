using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private const string PREFAB_PATH = "Prefabs/";

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>(PREFAB_PATH + path);
        if (prefab == null)
        {
            Debug.Log($"Failed to Load Prefab : {path}");
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
		go.name = prefab.name;

		return go;
    }
}
