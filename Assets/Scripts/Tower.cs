using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.Serialization;

public class Tower : MonoBehaviour
{
    private bool _isFire;
    private WaitForSeconds _wait=new (1);

    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private Transform firePoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
        {
           
            if (health.healthType==HealthType.Enemy)
            {
                Debug.Log("girdi");
                _isFire = true;
                StartCoroutine(Fire(health));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Health health))
        {
            if ((health.healthType==HealthType.Enemy))
            {
                _isFire = false;
                StopCoroutine(Fire(health));
            }
        }
    }

    

    IEnumerator Fire(Health other)
    {
        while (_isFire && other.health>0)
        {
            var bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
            var direction = other.transform.position+new Vector3(0,-5,0) - transform.position;
            bullet.GetComponent<Bullet>().InitBullet(direction,30,10);
            yield return _wait;
        }
    }
}
