using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    public static Managers Instance {
        get {
            if (s_instance == null)
            {
                s_instance = FindFirstObjectByType<Managers>();
                if (s_instance == null)
                {
                    GameObject go = new GameObject("Managers");
                    s_instance = go.AddComponent<Managers>();
                    DontDestroyOnLoad(go);
                }
            }
            return s_instance;
        }
    }

    // private InputManager _input;

    // public static InputManager Input => Instance._input ??= new InputManager();


    void OnDestroy()
    {
        Clear();
    }


    void Clear()
    {
        // 매니저들 정리 작업

    }

    void Update()
    {

    }
}
