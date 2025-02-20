using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 _startPosition;

    private void OnEnable()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player != null)
        {
            transform.position = player.transform.position;
            _startPosition = player.transform.position;
        }
    }

    void Update()
    {
        transform.position += transform.forward * 10.0f * Time.deltaTime;

        if (Vector3.Distance(_startPosition, transform.position) > 50.0f)
        {
            Debug.Log("Bullet Destroyed");
            Managers.Pool.Push(GetComponent<Poolable>());
        }
    }
}
