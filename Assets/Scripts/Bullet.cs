using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    //public GameObject bulletHitFeedBack;
    private float _speed;
    private float _despawnTime; 
    private int _damage;
    
    public void InitBullet(Vector3 direction, float speed, int damage, float despawnTime = 1f)
    {
        transform.forward = direction;
        _speed = speed;
        this._damage = damage;
        _despawnTime = despawnTime;
      
    }

   
    private void Update()
    {
        transform.Translate(transform.forward * (Time.deltaTime * _speed), Space.World);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(_damage);
        }

        OnHit();
    }
    
    private void OnHit()
    {
        gameObject.SetActive(false);
    }
}
