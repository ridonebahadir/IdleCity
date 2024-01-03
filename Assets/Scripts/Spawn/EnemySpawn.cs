
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
    
    [Space(10)]
    [Header("Wave")]
    [SerializeField] private int waveTime;
    [SerializeField] private Image waveSlider;
    [SerializeField] private TextMeshProUGUI waveText;

    
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _singletonHandler = SingletonHandler.Instance;
        SetSlider();
        
        // StartCoroutine(SpawnEnemyRoutine(1));
        // StartCoroutine(SpawnEnemyRoutine(2));
        // StartCoroutine(SpawnEnemyRoutine(3));
    }

    private void SetSlider()
    {
        waveText.SetText("Wave "+(waveCount).ToString());
        waveSlider.fillAmount = 1;
        waveSlider.DOFillAmount(0, waveTime).OnComplete(() =>
        {
            // m = 2+(w-1)*w/2
            var meleeCount = 2 + (waveCount - 1) * waveCount / 2;
            Debug.Log("Melee = " + meleeCount);
            Spawn(ObjectType.Enemy,meleeCount);
            
            //r = (w-1)*2-1
            var rangeCount = (waveCount - 1) * 2 - 1;
            Spawn(ObjectType.EnemyArcher,rangeCount);
            Debug.Log("Range = " + rangeCount);
            //g = w - 5
            var diggerCount = waveCount - 5;
            Spawn(ObjectType.EnemyDigger,diggerCount);
            Debug.Log("Giant = " + diggerCount);
            
            waveCount++;
            SetSlider();
          
        });
    }

    // private IEnumerator SpawnEnemyRoutine(int turn)
    // {
    //     WaitForSeconds waitForSeconds = new(waveTime);
    //     while (true)
    //     {
    //         yield return waitForSeconds;
    //         switch (turn)
    //         {
    //             case 1 :
    //                 // m = 2+(w-1)*w/2
    //                 var meleeCount = 2 + (waveCount - 1) * waveCount / 2;
    //                 Spawn(ObjectType.Enemy,meleeCount);
    //                 break;
    //             case 2:
    //                 //r = (w-1)*2-1
    //                 var rangeCount = (waveCount - 1) * 2 - 1;
    //                 Spawn(ObjectType.EnemyArcher,rangeCount);
    //                 break;
    //             case 3:
    //                 //g = w - 5
    //                 var diggerCount = waveCount - 5;
    //                 Spawn(ObjectType.EnemyDigger,diggerCount);
    //                 break;
    //         }
    //         
    //       
    //     }
    // }
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
