using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameObject _player;
    
    public GameObject GetPlayer() => _player;

    public GameObject Spawn(Define.ObjectType type, string path, Transform parent = null)
    {
        if (Managers.UGS.IsMultiPlay())
        {
            return SpawnFromServer(type, path, parent);
        }

        return SpawnLocally(type, path, parent);
    }

    private GameObject SpawnLocally(Define.ObjectType type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);
        
        if (go == null) {
            // 로깅
            return null;
        }

        switch (type)
        {
            case Define.ObjectType.Player:
                _player = go;
                break;
        }

        return go;
    }

    private GameObject SpawnFromServer(Define.ObjectType type, string path, Transform parent = null)
    {
        // 미구현
        return null;
    }
}
