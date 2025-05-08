using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeData
{
    [RecipeIDAttribute]
    public string recipeID;
    public string displayName;
    [BuildingIDAttribute]
    public string buildingID;
    [ItemIDAttribute]
    public string outputItemID;
    public int outputItemAmount;
    public int coinCost;
    public int timeCost;
    public string iconPath;

    public List<ResourceCost> inputResources;

    [System.NonSerialized]
    public Sprite iconSprite;
}

[System.Serializable]
public class ResourceCost
{
    [ItemIDAttribute]
    public string itemID;
    public int amount;
}
