using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.SampleScene;

        // SampleScene에서만 처리해야 할 Init 작업들
        GameObject player = Managers.Game.Spawn(Define.ObjectType.Player, "Player");
        if ( player != null && player.activeSelf ) {
            // 뭔가 한다
        }
        if ( player.IsValid() ) {
            // 뭔가 한다
        }
        player.GetOrAddComponent<PlayerController>();

        // 플레이어 위치 정해 주기

        // BGM

        // UI 같은 것들 미리 세팅

        // 탄막 100개를 미리 로드
    }
}
