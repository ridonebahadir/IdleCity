using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private GameManager _gameManager; 
    public SOSkillSettings soSkillSettings;
    
    [Header("Freeze Water")] 
    private float _freezeWaterActiveTime;
    private float _freezeWaterCoolTime;
    
    [Header("Heal Allies")] 
    private float _healAlliesActiveTime;
    private float _healCoolTime;
    private GameObject _angelObj;
    [SerializeField] private Transform angelSpawnPoint;
    
    
    [Header("Slow Enemies")] 
    private float _slowPercent;
    private float _slowActiveTime;
    private float _slowCoolTime;
    
    [Header("Attack Buff")] 
    private float _attackPercent;
    private float _attackActiveTime;
    private float _attackCoolTime;
    
    [Header("Deal Damage")] 
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
    [SerializeField] private Button slowEnemiesButton;
    [SerializeField] private Button attackBuffButton;
    [SerializeField] private Button dealDamageButton;
    [SerializeField] private Button increaseGoldButton;
    

    private void Start()
    {
        _gameManager = GameManager.Instance;
        healAlliesButton.onClick.AddListener(HealAllies);
        slowEnemiesButton.onClick.AddListener(SlowEnemies);
        attackBuffButton.onClick.AddListener(AttackBuff);
        dealDamageButton.onClick.AddListener(DealDamage);
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

        _slowPercent = soSkillSettings.slowPercent;
        _slowActiveTime = soSkillSettings.slowActiveTime;
        _slowCoolTime = soSkillSettings.slowCoolTime;

        _attackPercent = soSkillSettings.attackPercent;
        _attackActiveTime = soSkillSettings.attackActiveTime;
        _attackCoolTime = soSkillSettings.attackCoolTime;

        _dealDamagePercent = soSkillSettings.dealDamagePercent;
        _dealDamageCoolTime = soSkillSettings.dealDamageActiveTime;
        _dealDamageCoolTime = soSkillSettings.dealDamageCoolTime;
        _bomb = soSkillSettings.bomb;
        _bombCount = soSkillSettings.bombCount;
        

        _increaseGolPercent = soSkillSettings.increaseGolPercent;
        _increaseGoldCoolTime = soSkillSettings.increaseGoldCoolTime;
        _increaseGoldActiveTime = soSkillSettings.increaseGoldActiveTime;
    }

    private void FreezeWater()
    {
        ButtonClicked(freezeWaterButton,_freezeWaterActiveTime);
        StartCoroutine(FreezeWaterIe());
        return;

        IEnumerator FreezeWaterIe()
        {
            //_gameManager.dominationArea.getSplineFollower.enabled = false;
            _gameManager.dominationArea.enabled = false;
            yield return new WaitForSeconds(_freezeWaterActiveTime);
            _gameManager.dominationArea.enabled = true;
            //_gameManager.dominationArea.getSplineFollower.enabled = true;
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

    private void SlowEnemies() 
    {
        ButtonClicked(slowEnemiesButton,_slowActiveTime);
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
            ButtonClickAfter(slowEnemiesButton,_slowCoolTime);
        }
       
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

    private void DealDamage()
    {
        ButtonClicked(dealDamageButton,_dealDamageActiveTime);
        StartCoroutine(BombCreate());
        return;

        IEnumerator BombCreate()
        { 
            WaitForSeconds waitDealDamage = new(0.5f);
            for (var i = 0; i < _bombCount; i++)
            {
                var randomPos = (_gameManager.dominationArea.transform.position)+Random.insideUnitSphere * 15;
                randomPos.y = 50;
                Instantiate(_bomb, randomPos, Quaternion.identity);
                yield return waitDealDamage;
            }
            yield return new WaitForSeconds(_dealDamageActiveTime); 
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
