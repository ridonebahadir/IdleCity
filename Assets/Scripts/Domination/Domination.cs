using System.Collections;
using System.Collections.Generic;
using Agent;
using Dreamteck.Splines;
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
        public DominationMoveDirect dominationMoveDirect; 
        public List<Transform> enemiesSlot = new List<Transform>(); 
        public List<Transform> alliesSlot = new List<Transform>();
        public List<Transform> alliesArcherSlot = new List<Transform>();
        public List<Transform> enemiesArcherSlot = new List<Transform>();
        
        [SerializeField] private Transform sphere;
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private float speed;
        [SerializeField] private float riverWidth; 
        
        private float _sizeSpeed;
        private int _turn;
        private float _dist;
        private float _size;
        private float _goneRoad;
        private readonly WaitForSeconds _wait = new(0.01f);
        private SplinePoint[] _points;
        private IEnumerator _dominationMove; 
        private GameManager _gameManager;
        
        private void Start()
        {
            _points = splineComputer.GetPoints();
            Calculate();
            _dominationMove=DominationMove();
            StartCoroutine(_dominationMove);
            StartCoroutine(SetupDomination());
            _gameManager = GameManager.Instance;

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
                    splineFollower.followSpeed = 0;
                    dominationMoveDirect = DominationMoveDirect.None;
                    _start = true;
                    
                }
                yield return wait;
               
            }
        }
        private IEnumerator DominationMove()
        {
            while (true)
            {
                if (dominationMoveDirect == DominationMoveDirect.AlliesMove)
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
            speed = 0.5f;
            dominationMoveDirect = DominationMoveDirect.EnemyMove;
            StopCoroutine(_dominationMove);
            StartCoroutine(_dominationMove);
        }

        private void SoldierMove()
        {
            speed = 0.5f;
            dominationMoveDirect = DominationMoveDirect.AlliesMove;
            StopCoroutine(_dominationMove);
            StartCoroutine(_dominationMove);
        }
        private void Calculate()
        {
            if ( dominationMoveDirect == DominationMoveDirect.AlliesMove)
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
            if (!isMove)
            {
                captureTime -= Time.deltaTime;
                if (captureTime<=0)
                {
                    captureTime = 0;
                    if (!isWin)
                    {
                        EnemyMove();
                        _gameManager.GoDominationArea(false);
                        isMove = true;
                    }
                    else
                    {
                        SoldierMove();
                        _gameManager.GoDominationArea(true);
                        isMove = true;
                    }
                }
                else
                {
                    speed = 0;
                }
            }
           


        }

        [SerializeField] private float captureTime = 3;
        [SerializeField] private bool isWin;
        [SerializeField] private bool isMove = true;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SmallTrigger small))
            {
                
                if (small._agentType==AgentType.Enemy)
                {
                    if (!isWin)
                    {
                        
                    }
                    else
                    {
                        captureTime = 3;
                        isMove = false;
                    }
                    isWin = false;
                   
                }
                else
                {
                    if (isWin)
                    {
                        
                    }
                    else
                    {
                        captureTime = 3;
                        isMove = false;
                    }
                    isWin = true;
                }
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
       
    }
}
