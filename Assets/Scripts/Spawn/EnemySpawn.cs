
using System.Collections;
using Agent;
using DG.Tweening;
using LeonBrave;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
    private GameManager _gameManager;
    private Domination.Domination _domination;
    [SerializeField] private int waveCount;
    private SingletonHandler _singletonHandler;
    private int gameLevel;
    
    [Space(10)]
    [Header("Wave")]
    [SerializeField] private int waveTime;
    [SerializeField] private Image waveSlider;
    [SerializeField] private TextMeshProUGUI waveText;

    
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _singletonHandler = SingletonHandler.Instance;
        gameLevel = _gameManager.soGameSettings.level;
        SetSlider();
    }

    private void SetSlider()
    {
        waveText.SetText("Wave "+(waveCount).ToString());
        waveSlider.fillAmount = 0;
        waveSlider.DOFillAmount(1, waveTime).OnComplete(() =>
        {
            // m = 2+(w-1)*w/2
            var meleeCount = (2+gameLevel) + (waveCount - 1) * waveCount / 2;
            Debug.Log("Melee = " + meleeCount);
            Spawn(ObjectType.Enemy,meleeCount);
            
            //r = (w-1)*2-1
            var rangeCount = (waveCount - 1) * 2 - 1 + gameLevel;
            Spawn(ObjectType.EnemyArcher,rangeCount);
            Debug.Log("Range = " + rangeCount);
            //g = w - 5
            var diggerCount = waveCount - 5+gameLevel;
            Spawn(ObjectType.EnemyDigger,diggerCount);
            Debug.Log("Giant = " + diggerCount);
            
            waveCount++;
            SetSlider();
          
        });
    }
    private void Spawn(ObjectType objectType,int count)
    {
        if (count<=0) return;
        for (var i = 0; i < count; i++)
        {
            var cloneObj = _singletonHandler.GetSingleton<ObjectPool>().TakeObject(objectType);
            //var cloneObj= Instantiate(obj, pos.position,Quaternion.identity,pos);
            var rand = Random.Range(10, -10);
            cloneObj.transform.localPosition = new Vector3(rand, 0, 0);
            cloneObj.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
            cloneObj.SetActive(true);
            AgentBase agentBase = cloneObj.GetComponent<AgentBase>();
            agentBase.InıtAgent();
            _gameManager.enemies.Add(agentBase);
        }
      
    }
}
