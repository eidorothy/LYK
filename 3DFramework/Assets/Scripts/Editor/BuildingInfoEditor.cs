using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(BuildingInfo))]
public class BuildingInfoEditor : Editor
{
    [System.Serializable] public class RecipeArrayWrapper { public List<RecipeData> recipes; }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("자동 생성"))
        {
            var data = (BuildingInfo)target;
            data.buildingLevelDatas = new List<BuildingLevelData>
            {
                new BuildingLevelData
                {
                    level = 1,
                    buildCost = new List<ResourceCost>
                    {
                        new ResourceCost { itemID = "wood", amount = 10 }
                    },
                    recipeIDs = FindRecipeIDsForBuildingInfo(data.buildingID),
                },
            };
        }
    }

    private List<string> FindRecipeIDsForBuildingInfo(string buildingID)
    {
        TextAsset json = Resources.Load<TextAsset>("Data/recipe_data");
        var wrapper = JsonUtility.FromJson<RecipeArrayWrapper>(json.text);

        return wrapper.recipes
            .Where(r => r.buildingID == buildingID)
            .Select(r => r.recipeID)
            .ToList();
    }
}
