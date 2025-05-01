using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    private static Dictionary<string, ItemData> itemMap = new();

    public static void LoadFromJSON()
    {
        TextAsset json = Resources.Load<TextAsset>("Data/item_data");
        var array = JsonUtility.FromJson<ItemArrayWrapper>(json.text);

        foreach (var item in array.items)
        {
            item.iconSprite = Resources.Load<Sprite>(item.iconPath);
            itemMap[item.itemID] = item;
        }

        Debug.Log($"아이템 데이터 {itemMap.Count}개 완료!");
    }

    public static ItemData Get(string id) => itemMap.TryGetValue(id, out var data) ? data : null;

    [System.Serializable] public class ItemArrayWrapper { public List<ItemData> items; }
}
