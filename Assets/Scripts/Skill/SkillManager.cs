using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private Button freezeWaterButton;
    [SerializeField] private Button healAlliesButton;
    [SerializeField] private Button enemiesSlowButton;
    [SerializeField] private Button attackBuffButton;
    [SerializeField] private Button dealDamageButton;
    [SerializeField] private Button increaseGoldButton;
    

    private void Start()
    {
        _gameManager = GameManager.Instance;
        healAlliesButton.onClick.AddListener(HealAllies);
        enemiesSlowButton.onClick.AddListener(SlowEnemies);
        attackBuffButton.onClick.AddListener(AttackBuff);
        dealDamageButton.onClick.AddListener(DealDamage);
        increaseGoldButton.onClick.AddListener(IncreaseGold);
        freezeWaterButton.onClick.AddListener(FreezeWater);
    }

    private void HealAllies()
    {
        foreach (var item in _gameManager.soldiers)
        {
            item.SetPercentHealth(25);
        }
        
        // var a = _gameManager.soldiers.Count;
        // for (var i = 0; i < a; i++)
        // {
        //     _gameManager.soldiers[i].SetPercentHealth(25);
        // }
    }

    private void SlowEnemies() 
    {
        StartCoroutine(SlowEnemy());
        IEnumerator SlowEnemy()
        {
            
            foreach (var item in _gameManager.enemies)
            {
                item.SetPercentSpeed(-50);
            }
            yield return new WaitForSeconds(3);
            foreach (var item in _gameManager.enemies)
            {
                item.SetPercentSpeed(100);
            }
            
            
            // var a = _gameManager.enemies.Count;
            // for (var i = 0; i < a; i++)
            // {
            //     _gameManager.enemies[i].SetPercentSpeed(-50);
            // }
            // yield return new WaitForSeconds(3);
            // var b = _gameManager.enemies.Count;
            // for (var i = 0; i < b; i++)
            // {
            //     _gameManager.enemies[i].SetPercentSpeed(100);
            // }
        }
       
    }

    private void AttackBuff()
    {
        StartCoroutine(AttackBuffNumarator());
        IEnumerator AttackBuffNumarator()
        {
            foreach (var item in _gameManager.soldiers)
            {
                item.SetPercentAttack(30);
            }
            yield return new WaitForSeconds(3);
            foreach (var item in _gameManager.soldiers)
            {
                item.SetPercentAttack(-30);
            }
        }
       
    }

    private void DealDamage()
    {
        foreach (var item in _gameManager.enemies.ToArray())
        {
            item.SetPercentTakeDamage(20);
        }
    }

    private void IncreaseGold()
    {
        _gameManager.SetGoldRate( _gameManager.GetGoldRate);
    }

    private void FreezeWater()
    {
        StartCoroutine(FreezeWaterIe());

        IEnumerator FreezeWaterIe()
        {
             _gameManager.dominationArea.GetSplineFollower.enabled = false;
            yield return new WaitForSeconds(5);
            _gameManager.dominationArea.GetSplineFollower.enabled = true;
        }
        
    }
}
