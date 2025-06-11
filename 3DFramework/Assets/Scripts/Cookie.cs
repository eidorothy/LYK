using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cookie : MonoBehaviour, IBattleUnit
{
    [SerializeField]
    private CookieData cookieData;

    public string UnitID => cookieData.cookieID;
    public string DisplayName => cookieData.displayName;
    public Vector3 Position { get; set; }

    public int CurrentHP { get; set; }
    public int MaxHP => cookieData.baseHP;
    public int AttackPower => cookieData.baseAttackPower;
    public int DefensePower => cookieData.baseDefensePower;
    public float CriticalRate => cookieData.baseCriticalRate;
    public float CriticalDamage => cookieData.baseCriticalDamage;

    public float AttackCurCooltime { get; set; }
    public float AttackCooltime => cookieData.attackCooltime;
    public AttackType AttackType => cookieData.attackType;
    public float AttackRange => cookieData.attackRange;
    public TargetPriority AttackTargetPriority => cookieData.targetPriority;

    public SkillData SkillData => cookieData.skillData;
    public float SkillCurCooltime { get; set; }

    public float MoveSpeed => cookieData.moveSpeed;
    public bool IsMoving { get; set; }

    public bool IsAlive => CurrentHP > 0;

    public Action<IBattleUnit> OnDead { get; set; }
    public Action<IBattleUnit, int, bool> OnDamaged { get; set; }
    public Action<IBattleUnit, IBattleUnit, int, bool> OnAttack { get; set; }
    public Action<IBattleUnit, SkillData> OnSkillUsed { get; set; }

    public void Initialize()
    {
        CurrentHP = MaxHP;
        AttackCurCooltime = 0f;
        SkillCurCooltime = SkillData.cooltime;
        IsMoving = false;
        //Position = transform.position;
    }

    public void SetCookieData(CookieData data)
    {
        cookieData = data;
        Initialize();
    }

    public bool IsInAttackRange(IBattleUnit target)
    {
        float distance = Vector3.Distance(Position, target.Position);
        return distance <= AttackRange;
    }

    public bool IsInSkillRange(IBattleUnit target)
    {
        return IsInAttackRange(target);     // 쿠키는 기본적으로 평타와 동일한 범위로 설정
    }

    public void TakeDamage(int damage, bool isCritical = false)
    {
        int oldHP = CurrentHP;

        CurrentHP = Mathf.Max(0, CurrentHP - damage);

        OnDamaged?.Invoke(this, damage, isCritical);

        if (IsAlive == false)
        {
            Debug.Log($"{DisplayName}이(가) 쓰려졌습니다!");
            OnDead?.Invoke(this);
        }
    }

    public void TakeHeal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }

    public bool TryNormalAttack(IBattleUnit target, System.Random battleRandom)
    {
        if (!IsAlive || target == null || !target.IsAlive)
            return false;
        
        if (AttackCurCooltime > 0)
            return false;
        
        // 크리티컬 판정
        bool isCritical = battleRandom.NextDouble() < CriticalRate;

        // 데미지 계산
        float damage = AttackPower;
        if (isCritical)
        {
            damage *= CriticalDamage;
        }

        int finalDamage = Mathf.Max(0, (int)damage - target.DefensePower);  // 방어력 반영, 구하는 공식 바꿔도 됨

        target.TakeDamage(finalDamage, isCritical);
        AttackCurCooltime = AttackCooltime;

        string criText = isCritical ? " [크리티컬!]" : "";
        Debug.Log($"{DisplayName}이(가) {target.DisplayName} 공격! 데미지: {finalDamage}{criText} (적 HP: {target.CurrentHP}/{target.MaxHP})");

        OnAttack?.Invoke(this, target, finalDamage, isCritical);
        return true;
    }

    public bool TrySkillAttack(List<IBattleUnit> targets, System.Random battleRandom)
    {
        if (!IsAlive || targets == null || targets.Count == 0)
            return false;
        
        if (SkillCurCooltime > 0)
            return false;

        foreach (var target in targets.Where(t => t.IsAlive))
        {
            if (SkillData.skillType == SkillType.Damage)
            {
                // 크리티컬 판정
                bool isCritical = battleRandom.NextDouble() < CriticalRate;

                // 데미지 계산
                float damage = AttackPower * SkillData.damageMultiplier;
                if (isCritical)
                {
                    damage *= CriticalDamage;
                }

                int finalDamage = Mathf.Max(0, (int)damage - target.DefensePower);  // 방어력 반영, 구하는 공식 바꿔도 됨

                target.TakeDamage(finalDamage, isCritical);

                string criText = isCritical ? " [크리티컬!]" : "";
                Debug.Log($"{DisplayName}이(가) {target.DisplayName} 스킬 공격! 데미지: {finalDamage}{criText} (적 HP: {target.CurrentHP}/{target.MaxHP})");

            }
            else if (SkillData.skillType == SkillType.Heal)
            {
                int healAmount = Mathf.Max(0, (int)(AttackPower * SkillData.damageMultiplier));
                target.TakeHeal(healAmount);
                Debug.Log($"{DisplayName}이(가) {target.DisplayName}을(를) 치유! 치유량: {healAmount} (아군 HP: {target.CurrentHP}/{target.MaxHP})");
            }
        }
        
        SkillCurCooltime = SkillData.cooltime;        
        OnSkillUsed?.Invoke(this, SkillData);
        return true;
    }

    public void UpdateCooltime(float deltaTime)
    {
        if (AttackCurCooltime > 0)
            AttackCurCooltime -= deltaTime;

        if (SkillCurCooltime > 0)
            SkillCurCooltime -= deltaTime;
    }
}
