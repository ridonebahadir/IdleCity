using System;
using Agent;
using LeonBrave;
using UnityEngine;
using UnityEngine.Serialization;

public class Bomb : MonoBehaviour
{ 
   [SerializeField] private SOSkillSettings soSkillSettings;
   [SerializeField] private float moveSpeed;
   [SerializeField] private float lifeTime;
   [SerializeField] private ParticleSystem bombPlaneParticle;
   [SerializeField] private GameObject mesh;
   
   private SingletonHandler _singletonHandler;
   private bool _stop;
   private float _startLifeTime;
   
   public void Inıt(SingletonHandler singletonHandler)
   {
      _singletonHandler = singletonHandler;
      bombPlaneParticle.gameObject.SetActive(false);
      _stop = false;
      mesh.SetActive(true);
      _startLifeTime = lifeTime;
   }

   private void Update()
   {
      lifeTime -= Time.deltaTime;
      if (lifeTime<=0)
      {
         lifeTime = _startLifeTime;
         _singletonHandler.GetSingleton<ObjectPool>().AddObject(gameObject,ObjectType.Bomb);
         //Destroy(gameObject);
      }
      if (_stop) return;
      var movement = new Vector3(0f, -1, 0f) * (moveSpeed * Time.deltaTime);
      transform.Translate(movement);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.TryGetComponent(out AgentBase agentBase))
      {
         agentBase.SetPercentTakeDamage(soSkillSettings.dealDamagePercent);
      }

      _stop = true;
      mesh.SetActive(false);
     
      //bombPlaneParticle.transform.SetParent(null);
      bombPlaneParticle.gameObject.SetActive(true);
      
   }
   
}
