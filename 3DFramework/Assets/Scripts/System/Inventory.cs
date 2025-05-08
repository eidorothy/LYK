using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    private static Dictionary<string, int> _itemCounts = new Dictionary<string, int>();

    public static void AddItem(string itemName, int count = 1)
    {
        if (!_itemCounts.ContainsKey(itemName))
        {
            _itemCounts[itemName] = 0;
        }

        _itemCounts[itemName] += count;
    }

    public static int GetAmount(string itemID)
    {
        return _itemCounts.TryGetValue(itemID, out int count) ? count : 0;
    }

    public static bool HasEnough(string itemID, int required)
    {
        return GetAmount(itemID) >= required;
    }

    public static bool RemoveItem(string itemID, int count = 1)
    {
        if (!HasEnough(itemID, count))
            return false;
        _itemCounts[itemID] -= count;
        return true;
    }
}
