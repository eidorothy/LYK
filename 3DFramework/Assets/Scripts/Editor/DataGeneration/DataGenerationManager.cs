using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class DataGenerationManager
{
    private const string _sheetId = "1IlmlvnPg_rpGT6jEigeLQkz3p6gBFphq-0D52h24enE";
    private const string _itemSheetName = "Item";
    private const string _recipeSheetName = "Recipe";
    private const string _itemJsonPath = "Assets/Resources/Data/item_data.json";
    private const string _recipeJsonPath = "Assets/Resources/Data/recipe_data.json";
    private const string _itemHashPath = "Library/.item_data.hash";
    private const string _recipeHashPath = "Library/.recipe_data.hash";

    private const string _itemScriptPath = "Assets/Scripts/Data/Item/ItemID.cs";
    private const string _recipeScriptPath = "Assets/Scripts/Data/Recipe/RecipeID.cs";

    public static void GenerateAll()
    {
        bool itemChanged = DownloadIfChanged(_itemSheetName, _itemJsonPath, _itemHashPath, ParseItemCSV);
        bool recipeChanged = DownloadIfChanged(_recipeSheetName, _recipeJsonPath, _recipeHashPath, ParseRecipeCSV);

        if (itemChanged)
        {
            GenerateItemIDClass();
        }
        
        if (recipeChanged)
        {
            GenerateRecipeIDClass();
        }
    }

    private static bool DownloadIfChanged(string sheetName, string jsonPath, string hashPath, Func<string, string> parser)
    {
        string url = $"https://docs.google.com/spreadsheets/d/{_sheetId}/gviz/tq?tqx=out:csv&sheet={sheetName}";
        
        try {
            using WebClient client = new WebClient();
            byte[] rawData = client.DownloadData(url);
            string csv = Encoding.UTF8.GetString(rawData);
            string newHash = ComputeHash(csv);

            bool hashMatch = File.Exists(hashPath) && File.ReadAllText(hashPath) == newHash;
            bool jsonExists = File.Exists(jsonPath);

            if (hashMatch && jsonExists) {
                Debug.Log($"변경 없음! {Path.GetFileName(jsonPath)}");
                return false;
            }

            string json = parser(csv);
            
            File.WriteAllText(hashPath, newHash);   // csv 해시 갱신
            Debug.Log($"변환 완료! {sheetName}");
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError($"변환 실패! {sheetName}: {e.Message}");
            return false;
        }
    }

    private static string ComputeHash(string input)
    {
        using var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hash);
    }

    private static string ParseItemCSV(string csv)
    {
        //rollcake_wood,롤케이크 나무조각,lumberjack_house,wood,3,30,10
        List<ItemData> fullList = new();
        List<ItemIDData> idList = new();  // 진짜 ItemID만 가지고 있는 클래스
        string[] lines = csv.Split('\n');
        for (int i = 1; i < lines.Length; ++i)
        {
            var cols = lines[i].Split(',');

            var fullData = new ItemData
            {
                itemID = Clean(cols[0]),
                displayName = Clean(cols[1]),
                type = Clean(cols[2]),
                description = Clean(cols[3]),
                iconPath = Clean(cols[4])
            };
            fullList.Add(fullData);
            idList.Add(new ItemIDData{ itemID = fullData.itemID });
        }

        File.WriteAllText(_itemJsonPath, JsonUtility.ToJson(new ItemArrayWrapper{ items = fullList }, true), new UTF8Encoding(true));
        // return 값으로 json 데이터를 담은 string
        return JsonUtility.ToJson(new ItemArrayWrapper{ items = fullList }, true);
    }

    private static string ParseRecipeCSV(string csv)
    {
        //rollcake_wood,롤케이크 나무조각,lumberjack_house,wood,3,30,10
        List<RecipeData> fullList = new();
        List<RecipeIDData> idList = new();  // 진짜 ItemID만 가지고 있는 클래스
        string[] lines = csv.Split('\n');
        for (int i = 1; i < lines.Length; ++i)
        {
            var cols = lines[i].Split(',');

            var fullData = new RecipeData
            {
                recipeID = Clean(cols[0]),
                displayName = Clean(cols[1]),
                buildingID = Clean(cols[2]),
                outputItemID = Clean(cols[3]),
                outputItemAmount = SafeParseInt(Clean(cols[4])),
                coinCost = SafeParseInt(Clean(cols[5])),
                timeCost = SafeParseInt(Clean(cols[6])),
                iconPath = Clean(cols[7]),
                inputResources = new List<ResourceCost>()
            };

            if (!string.IsNullOrWhiteSpace(Clean(cols[8])))
                fullData.inputResources.Add(new ResourceCost{ itemID = Clean(cols[8]), amount = SafeParseInt(Clean(cols[9]))});

            if (!string.IsNullOrWhiteSpace(Clean(cols[10])))
                fullData.inputResources.Add(new ResourceCost{ itemID = Clean(cols[10]), amount = SafeParseInt(Clean(cols[11]))});

            fullList.Add(fullData);
            idList.Add(new RecipeIDData{ recipeID = fullData.recipeID });
        }

        File.WriteAllText(_recipeJsonPath, JsonUtility.ToJson(new RecipeArrayWrapper{ recipes = fullList }, true), new UTF8Encoding(true));
        // return 값으로 json 데이터를 담은 string
        return JsonUtility.ToJson(new RecipeArrayWrapper{ recipes = fullList }, true);
    }

    public static void GenerateItemIDClass() 
    {
        if (!File.Exists(_itemJsonPath))
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다! {_itemJsonPath}");
            return;
        }

        string json = File.ReadAllText(_itemJsonPath);
        ItemArrayWrapper data = JsonUtility.FromJson<ItemArrayWrapper>(json);

        if (data == null || data.items == null)
        {
            Debug.LogError($"JSON 파싱 실패! {_itemJsonPath}");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// 자동 생성된 아이템 ID 상수 목록");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class ItemID");
        sb.AppendLine("{");

        var keys = new HashSet<string>();   // 중복 허용하지 않음
        foreach (var item in data.items)
        {
            if (string.IsNullOrEmpty(item.itemID))
                continue;

            string key = ToPascalCase(item.itemID);
            if (!keys.Add(key))
            {
                Debug.LogError($"중복된 키! {key}");
                continue;
            }

            sb.AppendLine($"    public const string {key} = \"{item.itemID}\";");
        }

        sb.AppendLine();
        sb.AppendLine("    public static readonly string[] All = new[]");
        sb.AppendLine("    {");
        foreach (var item in data.items)
        {
            string key = ToPascalCase(item.itemID);
            sb.AppendLine($"        {key},");
        }
        sb.AppendLine("    };");
        sb.AppendLine("}"); 

        File.WriteAllText(_itemScriptPath, sb.ToString(), Encoding.UTF8);

        Debug.Log("ItemID.cs 생성 완료!");
    }

    public static void GenerateRecipeIDClass() 
    {
        if (!File.Exists(_recipeJsonPath))
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다! {_recipeJsonPath}");
            return;
        }

        string json = File.ReadAllText(_recipeJsonPath);
        RecipeArrayWrapper data = JsonUtility.FromJson<RecipeArrayWrapper>(json);

        if (data == null || data.recipes == null)
        {
            Debug.LogError($"JSON 파싱 실패! {_recipeJsonPath}");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// 자동 생성된 아이템 ID 상수 목록");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class RecipeID");
        sb.AppendLine("{");

        var keys = new HashSet<string>();   // 중복 허용하지 않음
        foreach (var recipe in data.recipes)
        {
            if (string.IsNullOrEmpty(recipe.recipeID))
                continue;

            string key = ToPascalCase(recipe.recipeID);
            if (!keys.Add(key))
            {
                Debug.LogError($"중복된 키! {key}");
                continue;
            }

            sb.AppendLine($"    public const string {key} = \"{recipe.recipeID}\";");
        }

        sb.AppendLine();
        sb.AppendLine("    public static readonly string[] All = new[]");
        sb.AppendLine("    {");
        foreach (var recipe in data.recipes)
        {
            string key = ToPascalCase(recipe.recipeID);
            sb.AppendLine($"        {key},");
        }
        sb.AppendLine("    };");
        sb.AppendLine("}"); 

        File.WriteAllText(_recipeScriptPath, sb.ToString(), Encoding.UTF8);

        Debug.Log("RecipeID.cs 생성 완료!");
    }

    private static string ToPascalCase(string id)
    {
        string[] parts = id.Split('_');
        for (int i = 0; i < parts.Length; ++i)
            parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i][1..];
        return string.Join("", parts);
    }

    private static string Clean(string raw) => raw.Trim().Trim('"');
    private static int SafeParseInt(string value, int defaultValue = 0) => int.TryParse(value, out var result) ? result : defaultValue;

    [System.Serializable] public class ItemArrayWrapper { public List<ItemData> items; }
    [System.Serializable] public class RecipeArrayWrapper { public List<RecipeData> recipes; }
}

[System.Serializable]
public class ItemIDData
{
    public string itemID;
}

[System.Serializable]
public class RecipeIDData
{
    public string recipeID;
}

// 빌드 시 자동 실행
public class DataAutoBuildPreprocesser : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        DataGenerationManager.GenerateAll();
    }
}

// 에디터 시작 시 자동 실행
[InitializeOnLoad]
public static class DataAutoEditorStartUp
{
    static DataAutoEditorStartUp()
    {
        DataGenerationManager.GenerateAll();
    }
}