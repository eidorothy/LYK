using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    public Image _icon;
    public TMP_Text _amountText;
    private string _itemID;

    public void SetItem(string itemID)
    {
        _itemID = itemID;
        UpdateUI();

        Inventory.OnChanged -= OnInventoryChanged;
        Inventory.OnChanged += OnInventoryChanged;
    }

    void OnDestroy() => Inventory.OnChanged -= OnInventoryChanged;

    void OnInventoryChanged(string itemID)
    {
        if (_itemID == itemID)
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        ItemData data = ItemDatabase.Get(_itemID);
        int count = Inventory.Get(_itemID);
        Debug.Log($"UpdateUI: {data.itemID} {count}");

        _icon.sprite = data.iconSprite;
        _amountText.text = count.ToString();

        // 수량이 0개라면 슬롯 비활성화
        gameObject.SetActive(count > 0);
    }
}
