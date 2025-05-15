using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollViewTest : MonoBehaviour
{
    public GameObject _itemPrefab;
    public Transform _contentRoot;
    public int _itemCount = 2000;

    private void Start()
    {
        for (int i = 0; i < _itemCount; ++i)
        {
            GameObject item = Instantiate(_itemPrefab, _contentRoot);
            item.name = $"item_{i}";
            var label = item.GetComponentInChildren<TMP_Text>();
            if (label != null)
            {
                label.text = $"item_{i}";
            }
        }
    }
}
