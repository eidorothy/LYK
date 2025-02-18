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
                    GameObject go = new GameObject("@Managers");
                    s_instance = go.AddComponent<Managers>();
                    DontDestroyOnLoad(go);
                }
            }
            return s_instance;
        }
    }

    private SceneMangerEx _scene;
    private UGSManager _ugs;
    private ResourceManager _resource;
    private GameManager _game;

    public static SceneMangerEx Scene => Instance._scene ??= new SceneMangerEx();
    public static UGSManager UGS => Instance._ugs ??= new UGSManager();
    public static ResourceManager Resource => Instance._resource ??= new ResourceManager();
    public static GameManager Game => Instance._game ??= new GameManager();


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
