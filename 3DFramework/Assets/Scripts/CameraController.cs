using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Vector3 _delta = new Vector3(0, 1f, -4.5f);

    [SerializeField]
    GameObject _target = null;

    public float smoothTime = 0.2f;
    
    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;
        
        transform.position = _target.transform.position + _delta;
        transform.LookAt(_target.transform);
    }
}