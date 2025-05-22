using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    [ItemIDAttribute]
    public string _itemID = ItemID.Wood;
    public int _amount = 1;

    public void OnClickConsume()
    {
        if (Inventory.HasEnough(_itemID, _amount))
        {
            Inventory.Consume(_itemID, _amount);
            Debug.Log($"{_itemID} {_amount}개 사용! 남은 수량 : {Inventory.Get(_itemID)}");
        }
        else
        {
            Debug.Log($"{_itemID}가 부족합니다! 현재 수량 : {Inventory.Get(_itemID)}");
        }
    }

    public void OnClickAdd()
    {
        Inventory.Add(_itemID, _amount);
        Debug.Log($"{_itemID} {_amount}개 추가! 현재 수량 : {Inventory.Get(_itemID)}");
    }
}
