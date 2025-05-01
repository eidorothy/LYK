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

        ItemDatabase.LoadFromJSON();        // 매니저에서 호출 권장
        var item = ItemDatabase.Get(ItemID.Wood);
        if (item != null)
            Debug.Log($"아이템 {item.displayName} {item.description}");
        else 
            Debug.Log("슬프다");
    }
}
