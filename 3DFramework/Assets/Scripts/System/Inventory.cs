using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    private static Dictionary<string, int> _itemCounts = new Dictionary<string, int>();

    public static void Add(string itemID, int count = 1)
    {
        if (!_itemCounts.ContainsKey(itemID))
        {
            _itemCounts[itemID] = 0;
        }

        _itemCounts[itemID] += count;
        OnChanged?.Invoke(itemID);        // UI에 알림
    }

    public static int Get(string itemID)
    {
        return _itemCounts.TryGetValue(itemID, out int count) ? count : 0;
    }

    public static bool HasEnough(string itemID, int required)
    {
        return Get(itemID) >= required;
    }

    public static void Consume(string itemID, int count = 1)
    {
        if (!HasEnough(itemID, count))
            return;
        _itemCounts[itemID] -= count;
        /*
        if (_itemCounts[itemID] <= 0)
        {
            _itemCounts.Remove(itemID);
        }
        */
        OnChanged?.Invoke(itemID);        // UI에 알림
    }

    public static event System.Action<string> OnChanged;

    public static IEnumerable<string> GetAll() => _itemCounts.Keys;
}
