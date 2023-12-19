using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using DG.Tweening;
using UnityEngine;

public class Bomb : MonoBehaviour
{
   [SerializeField] private SOSkillSettings _soSkillSettings;
   [SerializeField] private float moveSpeed;
   [SerializeField] private float lifeTime;
   [SerializeField] private ParticleSystem bombPlaneParticle;
   
   private void Update()
   {
      lifeTime -= Time.deltaTime;
      if (lifeTime<=0)
      {
         Destroy(gameObject);
      }
      Vector3 movement = new Vector3(0f, -1, 0f) * (moveSpeed * Time.deltaTime);
      transform.Translate(movement);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.TryGetComponent(out AgentBase agentBase))
      {
         agentBase.SetPercentTakeDamage(_soSkillSettings.dealDamagePercent);
      }
      bombPlaneParticle.transform.SetParent(null);
      bombPlaneParticle.gameObject.SetActive(true);
      
   }
   
}
