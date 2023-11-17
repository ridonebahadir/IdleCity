using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WoodMachine : MonoBehaviour
{
    [SerializeField] private SOWoodSkill woodSkill;
    [SerializeField] private GameObject slot;
    [SerializeField] private Transform outPut; 
    [SerializeField] private Transform inPut; 
    [SerializeField] private int outPutCount;
    
    public List<Transform> _outPutSlots = new List<Transform>();
    public List<Transform> _outPutToMoneySlots = new List<Transform>();
    private List<Transform> _inPutSlots = new List<Transform>();
    private float _multiplier;
    private void Start()
    {
        SpawnSlot();
        StartCoroutine(MachineRun());
        StartCoroutine(OutPutToMoney());
    }

    private void SpawnSlot()
    {
        //OutPut
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    var obj= Instantiate(slot, outPut);
                    obj.transform.localPosition = new Vector3(k, j, i);
                    _outPutSlots.Add(obj.transform);
                }
              
            }
        }
        //InPut
        for (int i = 0; i < 24; i++)
        {
            var outPutObj= Instantiate(slot, inPut);
            outPutObj.transform.localPosition = new Vector3(Random.insideUnitCircle.x,0,Random.insideUnitCircle.y)*4;
            _inPutSlots.Add(outPutObj.transform);
            var inPutObj=Instantiate(woodSkill.inPutObj,_inPutSlots[i]);
            inPutObj.transform.localPosition = Vector3.zero;
           
        }
    }

    IEnumerator MachineRun()
    {
        while (true)
        {
            if (outPutCount < 24)
            {
                InPut(); 
                OutPut();
                
            }
            _multiplier = woodSkill.defaultTime/woodSkill.machineSpeed/woodSkill.humanCount/woodSkill.waterSkill.flowSpeed;
            yield return new WaitForSeconds(_multiplier);

        }
        
    }

    
    private void InPut()
    {
        var obj = _inPutSlots[Random.Range(0, _inPutSlots.Count)];
        _inPutSlots.Remove(obj);
        obj.transform.DOScaleY(0, 0.5f).OnComplete(()=>
        {
            InPutRefill(obj);
        });
        
    }
    private void InPutRefill(Transform obj)
    {
       
        obj.transform.localPosition=new Vector3(Random.insideUnitCircle.x,0,Random.insideUnitCircle.y)*4;
        obj.transform.DOScaleY(1, 0.5f).OnComplete(()=>_inPutSlots.Add(obj));
    }
    private void OutPut()
    {
        var obj = Instantiate(woodSkill.outPutObj, transform.position, Quaternion.identity,_outPutSlots[outPutCount]);
        obj.transform.DOLocalJump(Vector3.zero, 5, 0, 0.5f);
        _outPutToMoneySlots.Add(obj.transform);
        outPutCount++;
    }
    IEnumerator OutPutToMoney()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(_multiplier);
            if (_outPutToMoneySlots.Count % woodSkill.earnMoneyRate == 0)
            {
                for (int i = 0; i <  woodSkill.earnMoneyRate; i++)
                {
                    var obj = _outPutToMoneySlots[^1];
                    outPutCount--;
                    _outPutToMoneySlots.Remove(obj);
                    obj.transform.DOJump(Vector3.zero, 3, 0, 0.5f).OnComplete(()=>obj.gameObject.SetActive(false));
                    yield return new WaitForSeconds(0.1f);
                }
                
            }
           
        }
       
    }
}
