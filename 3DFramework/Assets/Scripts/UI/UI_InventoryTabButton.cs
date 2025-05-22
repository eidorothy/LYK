using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryTabButton : MonoBehaviour
{
    public ItemCategory _category;
    public UI_Inventory _inventoryUI;

    private Toggle _toggle;

    void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            _inventoryUI.SetCategory(_category);    // 탭이 선택됐으니까, 카테고리 필터 적용
        }
    }
}
