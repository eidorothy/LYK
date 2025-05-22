using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory { All, Material, Goods, Etc }

public class UI_Inventory : MonoBehaviour
{
    public Transform _slotRoot;
    public GameObject _slotPrefab;
    public ItemCategory _category = ItemCategory.All;

    private List<string> _currentCategoryItems = new();
    private Dictionary<string, UI_InventorySlot> _itemSlots = new();

    void OnEnable()
    {
        Inventory.OnChanged -= OnInventoryChanged;
        Inventory.OnChanged += OnInventoryChanged;
    }

    void OnDisable()
    {
        Inventory.OnChanged -= OnInventoryChanged;
    }

    void Start()
    {
        SetCategory(ItemCategory.All);
    }

    private void OnInventoryChanged(string itemID)
    {
        if (_itemSlots.TryGetValue(itemID, out var slot))
        {
            slot.UpdateUI();    // 특정 슬롯만 Update

            if (Inventory.Get(itemID) == 0)
            {
                _itemSlots.Remove(itemID);
            }
        }
        else
        {
            Refresh();
        }
    }

    public void SetCategory(ItemCategory category)
    {
        _category = category;
        Refresh();
    }

    private List<string> ApplyCategory(IEnumerable<string> allItems)
    {
        List<string> result = new List<string>();

        foreach (var itemID in allItems)
        {
            var metaData = ItemDatabase.Get(itemID);
            if (metaData == null)
                continue;
            if (Inventory.Get(itemID) == 0)
                continue;
            if (_category == ItemCategory.All || metaData.type == _category.ToString().ToLower())
                result.Add(itemID);
        }

        return result;
    }

    public void Refresh()
    {
        // 데이터 구성
        var all = Inventory.GetAll();
        _currentCategoryItems = ApplyCategory(all); // wood, stone ...

        // 실제 UI에 반영
        UpdateUI();
    }

    public void UpdateUI()
    {
        _itemSlots.Clear();     // 다시 그리는 거니까 초기화, 실제로 보여지는 애들만 Dictionary에 넣어야 함

        for (int i = 0; i < _currentCategoryItems.Count; ++i)
        {
            var itemID = _currentCategoryItems[i];

            UI_InventorySlot slot;
            if (i < _slotRoot.childCount)
                slot = _slotRoot.GetChild(i).GetComponent<UI_InventorySlot>();  // 재활용
            else
                slot = Instantiate(_slotPrefab, _slotRoot).GetComponent<UI_InventorySlot>();

            slot.SetItem(itemID);
            slot.gameObject.SetActive(true);

            _itemSlots[itemID] = slot;
        }

        for (int i = _currentCategoryItems.Count; i < _slotRoot.childCount; ++i)
        {
            _slotRoot.GetChild(i).gameObject.SetActive(false);  // 재활용 안한 애들 비활성화
        }
    }
}
