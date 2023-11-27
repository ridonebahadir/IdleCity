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
        //[SerializeField] private SplinePositioner splinePositioner;
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private float speed;
        [SerializeField] private float riverWidth; 
        public List<AgentBase> enemies = new List<AgentBase>(); 
        public List<AgentBase> soldiers = new List<AgentBase>();
        
        //private float _currentDistance;
        private float _sizeSpeed;
        private int _turn;
        private float _dist;
        private float _size;
        private float _goneRoad;
        private readonly WaitForSeconds _wait = new(0.01f);
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

                if (_turn == 3)
                {
                    speed = 0;
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
                    //_currentDistance += Time.deltaTime*speed;
                    splineFollower.followSpeed = speed;
                    _goneRoad += Time.deltaTime*speed;
                    if (_size<=riverWidth) _size += Time.deltaTime * _sizeSpeed; 
                    if (_goneRoad>=_dist)
                    {
                        _size = 0.1f;
                        _goneRoad = 0;
                        if (_turn == _points.Length - 3)
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
                    //_currentDistance -= Time.deltaTime*speed;
                    splineFollower.followSpeed = -speed;
                    _goneRoad -= Time.deltaTime*speed;
                    if (_size>=0.1f) _size -= (Time.deltaTime * _sizeSpeed);
          
                    if (_goneRoad<=0)
                    {
                        if (_turn == 1)
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
                //splinePositioner.SetDistance(_currentDistance); 
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
            if (enemies.Count > 0 && soldiers.Count > 0)
            {
                splineFollower.followSpeed = 0;
                StopCoroutine(_dominationMove);
               
            }

            if (enemies.Count == 0 && soldiers.Count == 0)
            {
                splineFollower.followSpeed = 0;
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
                    if (!enemies.Contains(agentBase))
                    {
                        enemies.Add(agentBase);
                        if (!_isWin) speed = TotalDiggerSpeed(enemies);
                    }
                    if (enemies.Count==1) EnemyMove();
                    if (enemies.Count <= 0 || soldiers.Count <= 0) return;
                    AttackEnemy(); 
                    AttackSoldier();
                    break;
                }
                case AgentType.Soldier:
                {
                    _isWin = true;
                    if (!soldiers.Contains(agentBase))
                    {
                        soldiers.Add(agentBase);
                        if (_isWin)  speed = TotalDiggerSpeed(soldiers);
                    }
                    if (soldiers.Count==1) SoldierMove();
                    if (enemies.Count <= 0 || soldiers.Count <= 0) return;
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
                    if (enemies.Contains(agentBase)) RemoveList(agentBase,AgentType.Enemy);
                    break;
                case AgentType.Soldier:
                    if (soldiers.Contains(agentBase)) RemoveList(agentBase,AgentType.Soldier);
                    break;
            }
        }

        private void AttackEnemy()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Attack(CloseAgentSoldier(transform));
            }
        }
        private void AttackSoldier()
        {
         
            for (int i = 0; i < soldiers.Count; i++)
            {
                soldiers[i].Attack(CloseAgentEnemy(transform));
            }
         
        }
        
        public void RemoveList(AgentBase agentBase,AgentType agentType)
        {
            agentBase.isInside = false;
            switch (agentType)
            {
                case AgentType.Enemy:
                {
                    enemies.Remove(agentBase);
                    if (enemies.Count == 0)
                    {
                        speed = TotalDiggerSpeed(soldiers);
                        SoldierMove();
                    }
                    break;
                }
                case AgentType.Soldier:
                {
                    soldiers.Remove(agentBase);
                    if (soldiers.Count == 0)
                    {
                        speed = TotalDiggerSpeed(enemies);
                        EnemyMove();
                    }
                    break;
                }
            }
        }

        public Transform CloseAgentEnemy(Transform who)
        {
            return enemies.OrderBy(go => (who.position - go.transform.position).sqrMagnitude).First().transform;
        }

        public Transform CloseAgentSoldier(Transform who)
        {
            return soldiers.OrderBy(go => (who.position - go.transform.position).sqrMagnitude).First().transform;
        }

        float TotalDiggerSpeed(List<AgentBase> agentBases)
        {
            float a = 0;
            foreach (var item in agentBases)
            {
                a += item.DiggSpeed;
            }

            return a;
        }
       
    }
}
