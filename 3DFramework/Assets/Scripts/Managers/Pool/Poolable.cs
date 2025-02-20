using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public bool isUsing;

    public void OnPop()
    {
        gameObject.SetActive(true);
    }

    public void OnPush()
    {
        gameObject.SetActive(false);
    }
}
