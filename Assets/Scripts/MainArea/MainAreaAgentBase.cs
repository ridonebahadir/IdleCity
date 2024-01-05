using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class MainAreaAgentBase : MonoBehaviour
{
    [SerializeField] private CharactersType _charactersType;
    
     private NavMeshAgent _navMeshAgent;
     private Transform _target;
     [SerializeField]  private float _waitTime;
     [SerializeField]  private int _turn;
    [SerializeField] private List<Targets> targets;
    [SerializeField] private SOAgentUpgrade soAgentUpgrade;
    [SerializeField] private Transform levelParent;
     private Animator _animator;
     public bool _isAttack;
    
    private void Start()
    {
        SetLevelMesh();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        SetTarget();
    }

    private void SetLevelMesh()
    {
        foreach (Transform item in levelParent)
        {
            item.gameObject.SetActive(false);
        }
        var obj = levelParent.GetChild(soAgentUpgrade.level-1);
        obj.gameObject.SetActive(true);
        _animator = obj.GetComponent<Animator>();
    }

    private void SetTarget()
    {
        _isAttack = false;
        _coroutine = null;
        _target = targets[_turn].targetTrans;
        _waitTime = targets[_turn].waitTime;
        
        _animator.SetBool("Attack",false);
        _animator.SetBool("Wait",false);
        
        if (_one==null)
        {
            _one = One();
            StartCoroutine(_one);
        }
        if (_turn >= targets.Count-1)
        {
            
            _turn = 0;
            return;
        }
       
        _turn++;
    }

    private IEnumerator _zero;
    private IEnumerator _one;
    private void Update()
    {
        var posTarget = _target.position;
        _navMeshAgent.SetDestination(posTarget);
        
        var dist = Vector3.Distance(transform.position, posTarget);
        if (!(dist <= 2)) return;
        
        var targetRotation = _target.rotation;
        targetRotation.x = 0f;
        targetRotation.z = 0f;
        
        levelParent.rotation = targetRotation;
        if (_turn==1)
        {
            if (_zero==null)
            {
                _zero = Zero();
                StartCoroutine(_zero);
            }
            
        }
        
        _waitTime -= Time.deltaTime;
        
        if (_waitTime<=0)
        {
            SetTarget();
            return;
        }
        SetAnim();

    }

    private IEnumerator Zero()
    {
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            _zero = null;
        });
        yield return null;
    }
    private IEnumerator One()
    {
        levelParent.transform.DOLocalRotate(Vector3.zero, 0.25f);
        transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            _one = null;
        });
        yield return null;
    }

    private IEnumerator _coroutine;
    
    private void SetAnim()
    {
        if (_coroutine==null)
        {
            _coroutine =Anim();
            StartCoroutine(_coroutine);
        }
        
       
    }

    IEnumerator Anim()
    {
        switch (_turn)
        {
            case 0:
                _isAttack = true;
                Attack();
                _animator.SetBool("Attack",true);
                _animator.SetBool("Wait",false);
                break;
            case 1:
               _animator.SetBool("Wait",true);
               _animator.SetBool("Attack",false);
                break;
        }
        yield return null;
    }

    protected virtual void Attack()
    {
       
    }

    private void OnEnable()
    {
        switch (_charactersType)
        {
            case CharactersType.Melee:
                CharacterUpgradePanel.onClickUpgradeMelee += SetLevelMesh;
                break;
            case CharactersType.Archer:
                CharacterUpgradePanel.onClickUpgradeArcher += SetLevelMesh;
                break;
            case CharactersType.Digger:
                CharacterUpgradePanel.onClickUpgradeDigger += SetLevelMesh;
                break;
        }
        
    }

    private void OnDisable()
    {
        switch (_charactersType)
        {
            case CharactersType.Melee:
                CharacterUpgradePanel.onClickUpgradeMelee -= SetLevelMesh;
                break;
            case CharactersType.Archer:
                CharacterUpgradePanel.onClickUpgradeArcher -= SetLevelMesh;
                break;
            case CharactersType.Digger:
                CharacterUpgradePanel.onClickUpgradeDigger -= SetLevelMesh;
                break;
            case CharactersType.Giant:
                CharacterUpgradePanel.onClickUpgradeDigger -= SetLevelMesh;
                break;
        }
    }
}
[Serializable]
public struct Targets
{
    public Transform targetTrans;
    public float waitTime;

}

public enum CharactersType
{
    Melee,
    Archer,
    Digger,
    Giant,
    Town
}
