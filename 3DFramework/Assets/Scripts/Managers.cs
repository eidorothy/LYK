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

    private InputManager _input;

    public static InputManager Input => Instance._input ??= new InputManager();

/*
    // Managers도 Lazy Initialization
    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            //InitManagers();   // Lazy Initialization으로 변경
        }
    }
*/

    void OnDestroy()
    {
        Clear();
    }

    /*
    void InitManagers()
    {
        // 매니저들 생성
        //_input = new InputManager();
        //_game = new InputManager();
        //_ui = new UIManager();        // 나중에 사라져서 실제로는 아무데서도 안 쓰는 UI Manager 클래스
        //_input = new InputManager();
    }
    */

    void Clear()
    {
        // 매니저들 정리 작업
        _input?.Clear();
        //_game.Clear();
    }

    void Update()
    {
        _input?.OnUpdate();
    }
}
