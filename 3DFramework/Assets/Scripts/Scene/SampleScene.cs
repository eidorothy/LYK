using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScene : BaseScene
{
    [SerializeField]
    GameObject _bulletPrefab;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.SampleScene;

        // SampleScene에서만 처리해야 할 Init 작업들
        /*
        GameObject player = Managers.Game.Spawn(Define.ObjectType.Player, "Player");
        if ( player != null && player.activeSelf ) {
            // 뭔가 한다
        }
        if ( player.IsValid() ) {
            // 뭔가 한다
        }*/
        //player.GetOrAddComponent<PlayerController>();

        Managers.Pool.CreatePool(_bulletPrefab, 10);

        //Camera.main.gameObject.GetOrAddComponent<CameraController>().SetTarget(player);
        // BGM

        // UI 같은 것들 미리 세팅

        // 탄막 100개를 미리 로드
    }
}
