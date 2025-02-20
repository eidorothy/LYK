using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMangerEx
{
    // 외부에서 지금 어떤 씬인지 궁금해 하는 경우가 있어서 지금 어떤 씬인지 제공
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    // 씬 타입 - string
    string GetSceneName(Define.Scene sceneType)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), sceneType);
        return name;
    }

    // 씬을 로딩
    public void LoadScene(Define.Scene sceneType)
    {
        //string name = GetSceneName(Define.Scene.SampleScene);   // name == "SampleScene"
        string sceneName = GetSceneName(sceneType);

        if (SceneManager.GetActiveScene().name == sceneName)
            return;

        SceneManager.LoadScene(sceneName);
    }

    // 씬을 정리
    public void Clear()
    {
        // 씬 전체적으로 처리되어야 할 Clear
    }
}
