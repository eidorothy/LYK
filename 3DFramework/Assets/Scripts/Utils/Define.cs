using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookiePosition
{
    Front,
    Middle,
    Back,
}

public enum AttackType
{
    Melee,  // 근거리 공격
    Ranged, // 원거리 공격
    Heal,   // 치유
}

public enum TargetPriority
{
    Nearest,    // 가장 가까운 적
    Farthest,   // 가장 먼 적
    LowestHP,   // HP가 가장 낮은 적
}

public enum SkillType
{
    Damage,     // 피해를 주는 스킬
    Heal,       // 치유하는 스킬
    Buff,       // 버프를 주는 스킬
}

public enum SkillTargetType
{
    Single,             // 단일 대상
    MultipleEnemies,    // 여러 적 대상
    AllEnemies,         // 모든 적
    SingleAlly,         // 단일 아군 대상
    MultipleAllies,     // 여러 아군 대상
    AllAllies,          // 모든 아군 대상
    Self,               // 자신
}

// TODO : 도트 뎀, 독 데미지, 실드, 지속 회복, 이속 증가 등등 효과 관련 정의

public class Define
{
    public enum Scene
    {
        None,
        StartScene,
        SampleScene,
    }

    public enum ObjectType
    {
        None,
        Player,
        Bullet,
    }
}
