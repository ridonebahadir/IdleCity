using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRawImageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;
    [SerializeField] private List<SOAgentUpgrade> agentUpgrades;
    private readonly float _rotationSpeed = 15f;
    private Transform selectCharacter;
    public bool moving;
    private int _turn;
    void Start()
    {
        Close();
    }
    void Update()
    {
       
        if (selectCharacter != null)
        { 
            if (!moving)
            {
                Quaternion currentRotation = selectCharacter.localRotation;
                selectCharacter.localRotation = Quaternion.Lerp(currentRotation,  Quaternion.Euler(0,0,0), Time.deltaTime * _rotationSpeed);
                return;
            }
            var mouseX = Input.GetAxis("Mouse X");
            selectCharacter.Rotate(Vector3.up, mouseX * _rotationSpeed);
            
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                var touchDelta = touch.deltaPosition.x;
                transform.Rotate(Vector3.up, touchDelta * _rotationSpeed * Time.deltaTime);
            }
        };

    }
    private void Close()
    {
        foreach (var item in characters)
        {
            item.SetActive(false);
            item.transform.localRotation=Quaternion.Euler(0,0,0);
        }
    }

    private void OnEnable()
    {
        SelectCharacterUpgrade.onClickMelee += Open;
        SelectCharacterUpgrade.onClickArcher += Open;
        SelectCharacterUpgrade.onClickDigger += Open;

        RawImageButton.OnClickDown += SetMovingBool;
        RawImageButton.OnClickUp += SetMovingBool;
        
        //CharacterUpgradePanel.onClickUpgrade += SetModel;
        
    }

    private void OnDisable()
    {
        SelectCharacterUpgrade.onClickMelee -= Open;
        SelectCharacterUpgrade.onClickArcher -= Open;
        SelectCharacterUpgrade.onClickDigger -= Open;
        
        RawImageButton.OnClickDown -= SetMovingBool;
        RawImageButton.OnClickUp -= SetMovingBool;

        //CharacterUpgradePanel.onClickUpgrade -= SetModel;

    }

    private void Open(int a)
    {
        _turn = a;
        Close();
        selectCharacter = characters[a].transform;
        characters[a].SetActive(true);
    }

    private void SetModel()
    {
        foreach (Transform item in characters[_turn].transform)
        {
            foreach (Transform child in item)
            {
                child.transform.gameObject.SetActive(false);
            }
        }
        characters[_turn].transform.GetChild(0).transform.GetChild(agentUpgrades[_turn].level).gameObject.SetActive(true);
        characters[_turn].transform.GetChild(1).transform.GetChild(agentUpgrades[_turn].stage).gameObject.SetActive(true);
    }
    private void SetMovingBool()
    {
        moving = !moving;
    }
}
