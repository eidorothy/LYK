using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGSManager
{
    private static bool _isMultiPlay = false;

    public void SetMultiPlayMode(bool isMultiPlay)
    {
        _isMultiPlay = isMultiPlay;
    }

    public bool IsMultiPlay()
    {
        return _isMultiPlay;
    }
}
