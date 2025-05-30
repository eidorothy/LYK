using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCookie", menuName = "Game/CookieData")]
public class CookieData : ScriptableObject
{
    public string cookieID;
    public string displayName;

    public int baseHP;
    public int baseAttackPower;
    public int baseDefensePower;
    public float attackCooltime;    // 평타 쿨타임

    // TODO : 스킬 정보
}