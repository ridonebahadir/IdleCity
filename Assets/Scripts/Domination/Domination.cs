using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;


namespace Domination
{
    public enum DominationMoveDirect
    {
        AlliesMove,
        EnemyMove,
        None,
    }
    public class Domination : MonoBehaviour
    {
        public SplineFollower GetSplineFollower => splineFollower;
        public SplineComputer GetSplineComputer => splineComputer;
        public DominationMoveDirect dominationMoveDirect; 
        public List<Transform> enemiesSlot = new List<Transform>(); 
        public List<Transform> alliesSlot = new List<Transform>();
        public List<Transform> alliesArcherSlot = new List<Transform>();
        public List<Transform> enemiesArcherSlot = new List<Transform>();
        
        //[SerializeField] private Transform sphere;
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private SplineMesh splineMesh;
        [SerializeField] private float speed;
        //[SerializeField] private float riverWidth;
        [SerializeField] private TextMeshPro timeText;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Transform bridgeMesh;
        [SerializeField] private ParticleSystem circleParticle;
        private ParticleSystem.MainModule _mainModule;

        [SerializeField] public float captureTime = 3;
        [SerializeField] private bool isMove = true;
        [SerializeField] private List<SmallTrigger> enemies;
        [SerializeField] private List<SmallTrigger> allies;

        
        
        private bool _start;
        private GameManager _gameManager;
        
        private void Start()
        {
            ChangeParticleColor(Color.white);
            //splineComputer.SetPointSize(0,riverWidth);
            //_points = splineComputer.GetPoints();
            //Calculate();
            //_dominationMove=DominationMove();
            //StartCoroutine(_dominationMove);
            //StartCoroutine(SetupDomination());
            _gameManager = GameManager.Instance;

            

        }
        private void Update()
        {
            if (!_start) return;
            if (!isMove)
            {
                captureTime -= Time.deltaTime;
                var time = (int)captureTime;
                timeText.text = time.ToString();

               
                
                if (captureTime<=0f)
                {
                    if (dominationMoveDirect!=DominationMoveDirect.AlliesMove)
                    {
                        EnemyMove();
                        ChangeParticleColor(Color.red);
                        isMove = true;
                    }
                    if(dominationMoveDirect!=DominationMoveDirect.EnemyMove)
                    {
                        SoldierMove();
                        ChangeParticleColor(Color.blue);
                        isMove = true;
                    }
                    captureTime = 0;
                }
                else
                {
                   
                    speed = 0;
                }
            }

            var a = splineFollower.GetPercent();
            if (!_start) return;
            switch (a)
            {
                case 0f:
                    GameManager.Instance.uIManager.FailPanelOpen();
                    break;
                case 1f:
                    GameManager.Instance.uIManager.WinPanelOpen();
                    break;
            }
        }
        private void FixedUpdate()
        {
            if (dominationMoveDirect == DominationMoveDirect.AlliesMove)
            {
                bridgeMesh.transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
                splineFollower.followSpeed = speed;
            }

            if (dominationMoveDirect == DominationMoveDirect.EnemyMove)
            {
                bridgeMesh.transform.Rotate(-Vector3.right * (rotationSpeed * Time.deltaTime));
                splineFollower.followSpeed = -speed;
            }
            var a = splineFollower.GetPercent();
            splineMesh.SetClipRange(0,a);
            
                        
            
            if (_start) return;
            if (a < 0.35f) return;
            speed = 0;
            //StopCoroutine(_dominationMove);
            splineFollower.followSpeed = 0;
            dominationMoveDirect = DominationMoveDirect.None;
            _start = true;

           
        }
        private void EnemyMove()
        {
            speed = 0.5f;
            dominationMoveDirect = DominationMoveDirect.EnemyMove;
            _gameManager.GoDominationArea(true);
            // StopCoroutine(_dominationMove);
            // StartCoroutine(_dominationMove);
        }
        private void SoldierMove()
        {
            speed = 0.5f;
            dominationMoveDirect = DominationMoveDirect.AlliesMove;
            _gameManager.GoDominationArea(false);
            // StopCoroutine(_dominationMove);
            // StartCoroutine(_dominationMove);
        }
        private void ChangeParticleColor(Color color)
        {
            _mainModule = circleParticle.main;
            _mainModule.startColor = color;
            circleParticle.Play();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SmallTrigger small))
            {
                
                if (small._agentType==AgentType.Enemy)
                {
                    enemies.Add(small);
                   
                   
                }
                else
                {
                    allies.Add(small);
                    
                }
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (!other.transform.TryGetComponent(out SmallTrigger small)) return;
            if (enemies.Count > 0 && allies.Count > 0)
            {
                dominationMoveDirect = DominationMoveDirect.None;
                captureTime = 3;
            }
            else
            {
                if (enemies.Count>0)
                {
                    if (dominationMoveDirect!=DominationMoveDirect.EnemyMove)
                    {
                        captureTime = 3;
                        isMove = false;
                        dominationMoveDirect = DominationMoveDirect.EnemyMove;
                    }
                }

                if (allies.Count <= 0) return;
                if (dominationMoveDirect == DominationMoveDirect.AlliesMove) return;
                captureTime = 3;
                isMove = false;
                dominationMoveDirect = DominationMoveDirect.AlliesMove;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out SmallTrigger small))
            {
                UnRegister(small);
            }
        }
        public void UnRegister(SmallTrigger small)
        {
            
            if (small._agentType==AgentType.Enemy)
            {
                if (!enemies.Contains(small)) return;
                enemies.Remove(small);
            }
            else
            {
                if (!allies.Contains(small)) return;
                allies.Remove(small);
            }
        }


        private int _turnEnemies;
        private int _turnEnemiesArcher;
        private int _turnAllies;
        private int _turnAlliesArcher;
        public Transform SlotTarget(AgentType agentType)
        {
            if (agentType==AgentType.Enemy)
            {
                if (_turnEnemies<enemiesSlot.Count-1)  _turnEnemies++;
                else   _turnEnemies = 1;
                return enemiesSlot[_turnEnemies-1];
            }
            else
            {
                if (_turnAllies < alliesSlot.Count - 1) _turnAllies++;
                else _turnAllies = 1;
                return alliesSlot[_turnAllies-1];
            }
            
            
        }

        public void SlotTargetRemove(AgentType agentType)
        {
            if (agentType==AgentType.Enemy)
            {
                if (_turnEnemies>0)  _turnEnemies--;
                else   _turnEnemies = 0;
            }
            else
            {
                if (_turnAllies>0)  _turnAllies--;
                else   _turnAllies = 0;
            }
        }
        public Transform SlotArcherTarget(AgentType agentType)
        {
            if (agentType==AgentType.Enemy)
            {
                if (_turnEnemiesArcher<enemiesArcherSlot.Count-1)  _turnEnemiesArcher++;
                else   _turnEnemiesArcher = 1;
                return enemiesArcherSlot[_turnEnemiesArcher-1];
            }
            else
            {
                if (_turnAlliesArcher < alliesArcherSlot.Count - 1) _turnAlliesArcher++;
                else _turnAlliesArcher = 1;
                return alliesArcherSlot[_turnAlliesArcher-1];
            }

        }
        public void SlotTargetArcherRemove(AgentType agentType)
        {
            if (agentType==AgentType.Enemy)
            {
                if (_turnEnemiesArcher>0)  _turnEnemiesArcher--;
                else   _turnEnemiesArcher = 0;
            }
            else
            {
                if (_turnAlliesArcher>0)  _turnAlliesArcher--;
                else   _turnAlliesArcher = 0;
            }
        }
        
        
        
        
        
        // private IEnumerator SetupDomination()
        // {
        //     while (!_start)
        //     {
        //
        //         if (_turn == 3)
        //         {
        //             speed = 0;
        //             StopCoroutine(_dominationMove);
        //             splineFollower.followSpeed = 0;
        //             dominationMoveDirect = DominationMoveDirect.None;
        //             _start = true;
        //             
        //         }
        //         yield return _wait;
        //        
        //     }
        // }
        // private IEnumerator DominationMove()
        // {
        //     while (true)
        //     {
        //         if (dominationMoveDirect == DominationMoveDirect.AlliesMove)
        //         {
        //             //_currentDistance += Time.deltaTime*speed;
        //             bridgeMesh.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        //             splineFollower.followSpeed = speed;
        //             // _goneRoad += Time.deltaTime*speed;
        //             // if (_size<=riverWidth) _size += Time.deltaTime * _sizeSpeed; 
        //             // if (_goneRoad>=_dist)
        //             // {
        //             //     _size = 0.1f;
        //             //     _goneRoad = 0;
        //             //     if (_turn == _points.Length - 3)
        //             //     {
        //             //         Debug.Log("WIN");
        //             //         GameManager.Instance.uIManager.WinPanelOpen();
        //             //         break;
        //             //     }
        //             //     _turn++;
        //             //     Calculate();
        //             // }
        //         }
        //         if(dominationMoveDirect == DominationMoveDirect.EnemyMove)
        //         {
        //             //_currentDistance -= Time.deltaTime*speed;
        //             bridgeMesh.transform.Rotate(-Vector3.right * rotationSpeed * Time.deltaTime);
        //             splineFollower.followSpeed = -speed;
        //             // _goneRoad -= Time.deltaTime*speed;
        //             // if (_size>=0.1f) _size -= (Time.deltaTime * _sizeSpeed);
        //             //
        //             // if (_goneRoad<=0)
        //             // {
        //             //     if (_turn == 1)
        //             //     {
        //             //         Debug.Log("LOSE");
        //             //         GameManager.Instance.uIManager.FailPanelOpen();
        //             //         break;
        //             //     }
        //             //     _size = riverWidth;
        //             //     _turn--;
        //             //     Calculate();
        //             //     _goneRoad = _dist;
        //             //
        //             //
        //             // }
        //         }
        //         //splinePositioner.SetDistance(_currentDistance); 
        //         //splineComputer.SetPointSize(_turn,_size);
        //         var a = splineFollower.GetPercent();
        //         splineMesh.SetClipRange(0,a);
        //         yield return _wait;
        //     }
        //
        // }
        // private void Calculate()
        // {
        //     if ( dominationMoveDirect == DominationMoveDirect.AlliesMove)
        //     {
        //         _dist = Vector3.Distance(sphere.position, _points[_turn+1].position);
        //         _sizeSpeed = (riverWidth-(splineComputer.GetPoint(_turn).size)) / (_dist / speed);
        //     }
        //     else
        //     {
        //         _dist = Vector3.Distance(sphere.position, _points[_turn].position);
        //         _sizeSpeed = (splineComputer.GetPoint(_turn).size) / (_dist / speed);
        //     }
        //
        //  
        // }
       
    }
}
