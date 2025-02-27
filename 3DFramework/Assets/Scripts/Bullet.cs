using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 _startPosition;
    Rigidbody _rb;
    public float _bulletSpeed = 10.0f;
    public float _maxDistance = 50.0f;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();

        GameObject player = Managers.Game.GetPlayer();
        if (player != null)
        {
            transform.position = player.transform.position;
            _startPosition = player.transform.position;

            _rb.velocity = player.transform.forward *_bulletSpeed;

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Player"));
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(_startPosition, transform.position) > _maxDistance)
        {
            Debug.Log("Bullet Destroyed");
            Managers.Pool.Push(GetComponent<Poolable>());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("총알 충돌!! "+ collision.gameObject.name);

        // 플레이어 본인/적 HP 깎는 처리 등등...

        Managers.Pool.Push(GetComponent<Poolable>());
    }
    /*
    void Update()
    {
        transform.position += transform.forward * 10.0f * Time.deltaTime;

        if (Vector3.Distance(_startPosition, transform.position) > 50.0f)
        {
            Debug.Log("Bullet Destroyed");
            Managers.Pool.Push(GetComponent<Poolable>());
        }
    }
    */
}
