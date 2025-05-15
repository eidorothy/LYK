using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomScrollItem : MonoBehaviour
{
    public RectTransform Rect => (RectTransform)transform;
    private int _index = -1;

    public void SetData(int index)
    {
        if (_index == index)
        {
            return;
        }
        _index = index;

        var label = GetComponentInChildren<TMPro.TMP_Text>();
        if (label != null)
        {
            label.text = $"item_{index}";
        }
    }
}
