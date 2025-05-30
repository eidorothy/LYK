using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleUnit
{
    float AttackCooltime { get; }          // 데이터화된 평타 쿨타임 (3초)
    float AttackCurCooltime { get; set; }  // 런타임에서 현재 평타 쿨타임 (3초 -> 0초 -> 평타 -> 3초)
    // TODO : 평타 타입 (근거리냐, 원거리냐)
    // TODO : 평타 사거리 (가까이 다가가서 공격해야 하니까)

    // TODO : 스킬 관련 (데이터에 있는 찐 쿨타임(쿨타임:3초), 현재 쿨타임)

    int CurrentHP { get; set; } // 현재 HP
    int MaxHP { get; }          // 최대 HP

    int AttackPower { get; }    // 공격력
    int DefensePower { get; }   // 방어력
                                // TODO : 치명타 확률, 치명타 피해

    void Init();                // 초기화 함수

    void TakeDamge(int damage); // 데미지 받는 함수
    void TryNormalAttack(IBattleUnit target);           // 평타를 시도하는 함수
    void TrySkillAttack(List<IBattleUnit> targets);     // 스킬을 시도하는 함수
}