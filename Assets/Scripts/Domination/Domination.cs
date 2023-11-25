using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

namespace Domination
{
    public class Domination : MonoBehaviour
    {
        [SerializeField] private Transform sphere;
        [SerializeField] private SplinePositioner splinePositioner;
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private float speed;
        [SerializeField] private float riverWidth;
        public List<AgentBase> _enemies = new List<AgentBase>();
        public List<AgentBase> _soldiers = new List<AgentBase>();
        
        private float _currentDistance;
        private float _sizeSpeed; 
        private int _turn;
        private float _dist;
        private float _size;
        private float _goneRoad;
        private WaitForSeconds _wait = new(0.01f);
        private SplinePoint[] _points;
        private IEnumerator _dominationMove;
        private bool _isWin = true;
    
        private void Start()
        {
            _points = splineComputer.GetPoints();
            Calculate();
            _dominationMove=DominationMove();
            StartCoroutine(_dominationMove);
            StartCoroutine(SetupDomination());
            

        }

        private IEnumerator SetupDomination()
        {
            WaitForSeconds wait = new(0.25f);
            while (!_start)
            {

                if (_turn == 2)
                {
                    StopCoroutine(_dominationMove);
                    _start = true;
                    
                }
                yield return wait;
               
            }
        }
        private IEnumerator DominationMove()
        {
            while (true)
            {
                if (_isWin)
                {
                    _currentDistance += Time.deltaTime*speed;
                    _goneRoad += Time.deltaTime*speed;
                    if (_size<=riverWidth) _size += Time.deltaTime * _sizeSpeed; 
                    if (_goneRoad>=_dist)
                    {
                        _size = 0.1f;
                        _goneRoad = 0;
                        if (_turn == _points.Length - 2)
                        {
                            Debug.Log("WIN");
                            GameManager.Instance.uIManager.WinPanelOpen();
                            break;
                        }
                        _turn++;
                        Calculate();
                    }
                }
                else
                {
                    _currentDistance -= Time.deltaTime*speed;
                    _goneRoad -= Time.deltaTime*speed;
                    if (_size>=0.1f) _size -= (Time.deltaTime * _sizeSpeed);
          
                    if (_goneRoad<=0)
                    {
                        if (_turn == 0)
                        {
                            Debug.Log("LOSE");
                            GameManager.Instance.uIManager.FailPanelOpen();
                            break;
                        }
                        _size = riverWidth;
                        _turn--;
                        Calculate();
                        _goneRoad = _dist;
                   

                    }
                }
                splinePositioner.SetDistance(_currentDistance); 
                splineComputer.SetPointSize(_turn,_size);
                yield return _wait;
            }
       
        }
        private void EnemyMove()
        {
           
            _isWin = false;
            StopCoroutine(_dominationMove);
            StartCoroutine(_dominationMove);
        }

        private void SoldierMove()
        {
            
            _isWin = true;
            StopCoroutine(_dominationMove);
            StartCoroutine(_dominationMove);
        }
        private void Calculate()
        {
            if (_isWin)
            {
                _dist = Vector3.Distance(sphere.position, _points[_turn+1].position);
                _sizeSpeed = (riverWidth-(splineComputer.GetPoint(_turn).size)) / (_dist / speed);
            }
            else
            {
                _dist = Vector3.Distance(sphere.position, _points[_turn].position);
                _sizeSpeed = (splineComputer.GetPoint(_turn).size) / (_dist / speed);
            }
        
         
        }

        private bool _start;
        private void Update()
        {
            if (!_start) return;
            if (_enemies.Count > 0 && _soldiers.Count > 0)
            {
                StopCoroutine(_dominationMove);
            
            }

            if (_enemies.Count == 0 && _soldiers.Count == 0)
            {
                StopCoroutine(_dominationMove);
            
            }


        }
     
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AgentBase agentBase))
            {
                Register(agentBase);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out AgentBase agentBase))
            {
                UnRegister(agentBase);
            
            }
        
        }
     
        private void Register(AgentBase agentBase)
        {
            agentBase.isInside = true;
            switch (agentBase.soAgent.agentType)
            {
                case AgentType.Enemy:
                {
                    _isWin = false;
                    if (!_enemies.Contains(agentBase))
                    {
                        _enemies.Add(agentBase);
                        if (!_isWin) speed = _enemies.Count * 0.5f;
                    }
                    if (_enemies.Count==1) EnemyMove();
                    if (_enemies.Count <= 0 || _soldiers.Count <= 0) return;
                    AttackEnemy(); 
                    AttackSoldier();
                    break;
                }
                case AgentType.Soldier:
                {
                    _isWin = true;
                    if (!_soldiers.Contains(agentBase))
                    {
                        _soldiers.Add(agentBase);
                        if (_isWin)  speed = _soldiers.Count*0.5f;
                    }
                    if (_soldiers.Count==1) SoldierMove();
                    if (_enemies.Count <= 0 || _soldiers.Count <= 0) return;
                    AttackEnemy(); 
                    AttackSoldier();
                    break;
                }
            }
        }

        private void UnRegister(AgentBase agentBase)
        {
            switch (agentBase.soAgent.agentType)
            {
                case AgentType.Enemy:
                    if (_enemies.Contains(agentBase)) RemoveList(agentBase,AgentType.Enemy);
                    break;
                case AgentType.Soldier:
                    if (_soldiers.Contains(agentBase)) RemoveList(agentBase,AgentType.Soldier);
                    break;
            }
        }

        private void AttackEnemy()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Attack(CloseAgentSoldier(transform));
            }
        }
        private void AttackSoldier()
        {
         
            for (int i = 0; i < _soldiers.Count; i++)
            {
                _soldiers[i].Attack(CloseAgentEnemy(transform));
            }
         
        }
        
        public void RemoveList(AgentBase agentBase,AgentType agentType)
        {
            agentBase.isInside = false;
            switch (agentType)
            {
                case AgentType.Enemy:
                {
                    _enemies.Remove(agentBase);
                    if (_enemies.Count == 0)
                    {
                        speed = _soldiers.Count * 0.5f;
                        SoldierMove();
                    }
                    break;
                }
                case AgentType.Soldier:
                {
                    _soldiers.Remove(agentBase);
                    if (_soldiers.Count == 0)
                    {
                        speed = _enemies.Count*0.5f;
                        EnemyMove();
                    }
                    break;
                }
            }
        }

        public Transform CloseAgentEnemy(Transform who)
        {
            return _enemies.OrderBy(go => (who.position - go.transform.position).sqrMagnitude).First().transform;
        }

        public Transform CloseAgentSoldier(Transform who)
        {
            return _soldiers.OrderBy(go => (who.position - go.transform.position).sqrMagnitude).First().transform;
        }
        float _a = 0;
       
    }
}
