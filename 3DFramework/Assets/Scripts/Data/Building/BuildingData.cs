using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingLevelData
{
    public int level;
    public List<ResourceCost> buildCost;    // 건설 or 업그레이드 비용
    [RecipeIDAttribute]
    public List<string> recipeIDs;          // 생산할 수 있는 레시피 ID들
}

[CreateAssetMenu(fileName = "BuildingInfo", menuName = "Game/BuildingInfo", order = 1)]
public class BuildingInfo : ScriptableObject
{
    [BuildingIDAttribute]
    public string buildingID;       // 나무꾼 건물
    public string displayName;      // 나무꾼 건물
    public Sprite icon;

    public List<BuildingLevelData> buildingLevelDatas;
}