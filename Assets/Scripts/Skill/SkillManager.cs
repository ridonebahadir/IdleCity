using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using LeonBrave;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = System.Numerics.Vector3;

public class SkillManager : MonoBehaviour
{
    private GameManager _gameManager;
    private Domination.Domination _domination;
    private SingletonHandler _singletonHandler;
    public SOSkillSettings soSkillSettings;
    
    [Header("Freeze Water")] 
    private float _freezeWaterActiveTime;
    private float _freezeWaterCoolTime;
   
    
    [Header("Heal Allies")] 
    private float _healAlliesActiveTime;
    private float _healCoolTime;
    private GameObject _angelObj;
    [SerializeField] private Transform angelSpawnPoint;
    
    
    [Header("Devil Create")] 
    //private float _slowPercent;
    private float _devilActiveTime;
    private float _devilCoolTime;
    private GameObject _devilObj;
    [SerializeField] private Transform devilSpawnPoint;
    
    [Header("Attack Buff")] 
    private float _attackPercent;
    private float _attackActiveTime;
    private float _attackCoolTime;
    
    [Header("Meteor")] 
    private float _dealDamagePercent;
    private int _bombCount;
    private float _dealDamageActiveTime;
    private float _dealDamageCoolTime;
    private GameObject _bomb;
    
    [Header("Increase Gold")] 
    private float _increaseGolPercent;
    private float _increaseGoldActiveTime;
    private float _increaseGoldCoolTime;
    

    [SerializeField] private Button freezeWaterButton;
    [SerializeField] private Button healAlliesButton; 
    [SerializeField] private Button devilCreateButton;
    [SerializeField] private Button attackBuffButton;
    [SerializeField] private Button dealDamageButton;
    [SerializeField] private Button increaseGoldButton;
    

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _singletonHandler = SingletonHandler.Instance;
        _domination = _gameManager.dominationArea;
        healAlliesButton.onClick.AddListener(HealAllies);
        devilCreateButton.onClick.AddListener(DevilCreate);
        attackBuffButton.onClick.AddListener(AttackBuff);
        dealDamageButton.onClick.AddListener(Meteor);
        increaseGoldButton.onClick.AddListener(IncreaseGold);
        freezeWaterButton.onClick.AddListener(FreezeWater);
        InıtSoSkillSettings();
    }

    private void InıtSoSkillSettings()
    {
        _freezeWaterActiveTime = soSkillSettings.freezeWaterActiveTime;
        _freezeWaterCoolTime = soSkillSettings.freezeWaterCoolTime;
        
        _healAlliesActiveTime = soSkillSettings.healAlliesActiveTime;
        _healCoolTime = soSkillSettings.healCoolTime;
        _angelObj = soSkillSettings.angelObj;

        //_slowPercent = soSkillSettings.slowPercent;
        _devilActiveTime = soSkillSettings.devilActiveTime;
        _devilCoolTime = soSkillSettings.devilCoolTime;
        _devilObj = soSkillSettings.devilObj;

        _attackPercent = soSkillSettings.attackPercent;
        _attackActiveTime = soSkillSettings.attackActiveTime;
        _attackCoolTime = soSkillSettings.attackCoolTime;

        _dealDamagePercent = soSkillSettings.dealDamagePercent;
        _dealDamageActiveTime = soSkillSettings.dealDamageActiveTime;
        _dealDamageCoolTime = soSkillSettings.dealDamageCoolTime;
        _bomb = soSkillSettings.bomb;
        _bombCount = soSkillSettings.bombCount;
        

        _increaseGolPercent = soSkillSettings.increaseGolPercent;
        _increaseGoldCoolTime = soSkillSettings.increaseGoldCoolTime;
        _increaseGoldActiveTime = soSkillSettings.increaseGoldActiveTime;
    }
    
    private WaitForSeconds _freezeWaterDropWait = new(0.3f);

    /*public SkillManager(float slowPercent)
    {
        _slowPercent = slowPercent;
    }*/

    private void FreezeWater()
    {
        ButtonClicked(freezeWaterButton,_freezeWaterActiveTime);
        StartCoroutine(FreezeWaterIe());
        return;

        IEnumerator FreezeWaterIe()
        {
            
            _domination.GetSplineFollower.enabled = false;
            _domination.enabled = false;

            
            var splineComputer = _domination.GetSplineComputer;

            var point = FreezeWaterDropPoint.Create(soSkillSettings.freezeWaterPointObj,splineComputer,_domination);
            while (point!=null)
            {
                var obj = _singletonHandler.GetSingleton<ObjectPool>().TakeObject(ObjectType.FreeWaterDrop).GetComponent<FreeWaterDrop>();
                obj.transform.position = point.transform.position + (UnityEngine.Vector3.up * 50);
                obj.gameObject.SetActive(true);
                obj.Inıt(_singletonHandler,_freezeWaterActiveTime);
                yield return _freezeWaterDropWait;
            }
            yield return new WaitForSeconds(_freezeWaterActiveTime);
            _domination.enabled = true;
            _domination.GetSplineFollower.enabled = true;
            ButtonClickAfter(freezeWaterButton,_freezeWaterCoolTime);
        }
        
    }
    private void HealAllies()
    {
        if (_gameManager.soldiers.Count==0) return;
        ButtonClicked(healAlliesButton,_healAlliesActiveTime);
        Angel.Create(_angelObj, angelSpawnPoint, _gameManager.GetFurthestAllie(),_healAlliesActiveTime);
        StartCoroutine(ClickButton());
        return;

        IEnumerator ClickButton()
        {
            yield return new WaitForSeconds(_healAlliesActiveTime);
            ButtonClickAfter(healAlliesButton,_healCoolTime);
        }
        
    }
    private void DevilCreate()
    {
        if (_gameManager.enemies.Count==0) return;
        ButtonClicked(devilCreateButton,_devilActiveTime);
        Devil.Create(_devilObj, devilSpawnPoint, _gameManager.GetFurhestEnemies(),_devilActiveTime);
        StartCoroutine(ClickButton());
        return;

        IEnumerator ClickButton()
        {
            yield return new WaitForSeconds(_devilActiveTime);
            ButtonClickAfter(devilCreateButton,_devilCoolTime);
        }
        
    }

    private void SlowEnemies() 
    {
        /*//ButtonClicked(slowEnemiesButton,_slowActiveTime);
        StartCoroutine(SlowEnemy());
        return;

        IEnumerator SlowEnemy()
        {
            
            foreach (var item in _gameManager.enemies)
            {
                item.SetPercentSpeed(-_slowPercent);
            }
            yield return new WaitForSeconds(_slowActiveTime);
            foreach (var item in _gameManager.enemies)
            {
                item.SetPercentSpeed(100);
            }
            //ButtonClickAfter(slowEnemiesButton,_slowCoolTime);
        }*/
       
    }

    private void AttackBuff()
    {
        ButtonClicked(attackBuffButton,_attackActiveTime);
        StartCoroutine(AttackBuffNumarator());
        IEnumerator AttackBuffNumarator()
        {
            foreach (var item in _gameManager.soldiers)
            {
                item.SetPercentAttack(_attackPercent);
            }
            yield return new WaitForSeconds(_attackActiveTime);
            foreach (var item in _gameManager.soldiers)
            {
                item.SetPercentAttack(-_attackPercent);
            }
            ButtonClickAfter(attackBuffButton,_attackCoolTime);
        }
       
    }

    private void Meteor()
    {
        ButtonClicked(dealDamageButton,_dealDamageActiveTime);
        StartCoroutine(BombCreate());
        return;

        IEnumerator BombCreate()
        {
            const float waitTime = 0.2f;
            WaitForSeconds waitDealDamage = new(waitTime);
            var elapsedTime = 0f;
            var duration = _dealDamageActiveTime;
            while (elapsedTime < duration)
            {
                elapsedTime +=waitTime;
                var randomPos = (_gameManager.dominationArea.transform.position)+Random.insideUnitSphere * 15;
                randomPos.y = 50;
                var obj = _singletonHandler.GetSingleton<ObjectPool>().TakeObject(ObjectType.Bomb).transform.GetComponent<Bomb>();
                obj.Inıt(_singletonHandler);
                obj.transform.position = randomPos;
                obj.gameObject.SetActive(true);
                //Instantiate(_bomb, randomPos, Quaternion.identity);
                yield return waitDealDamage;
                
            }
            // return new WaitForSeconds(_dealDamageActiveTime)
            ButtonClickAfter(dealDamageButton,_dealDamageCoolTime);
        }
      
    }
    private void IncreaseGold()
    {
        ButtonClicked(increaseGoldButton,_increaseGoldActiveTime);
        StartCoroutine(IncreaseGoldIe());

        IEnumerator IncreaseGoldIe()
        {
            var a = (_increaseGolPercent * _gameManager.GetGoldRate) / 100;
            _gameManager.SetGoldRate(a);
            yield return new WaitForSeconds(_increaseGoldActiveTime);
            _gameManager.SetGoldRate(-a);
            ButtonClickAfter(increaseGoldButton,_increaseGoldCoolTime);
        }
       
       
    }

    private void ButtonClickAfter(Button button,float cooldown)
    {
        button.image.DOFillAmount(1, cooldown).OnComplete(() =>
        {
            button.interactable = true;
        });
    }
    private void ButtonClicked(Button button,float activeTime)
    {
        button.interactable = false;
        button.image.DOFillAmount(0, activeTime);
    }
    private void ButtonClick(Button button,float activeTime)
    {
        button.image.DOFillAmount(0, activeTime);
    }
    
  

    
}
