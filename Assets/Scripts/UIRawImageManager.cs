using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRawImageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;
    private readonly float _rotationSpeed = 15f;
    private Transform selectCharacter;
    public bool moving;
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
                selectCharacter.localRotation = Quaternion.Lerp(currentRotation,  Quaternion.Euler(0,145,0), Time.deltaTime * _rotationSpeed);
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
            item.transform.localRotation=Quaternion.Euler(0,145,0);
        }
    }

    private void OnEnable()
    {
        SelectCharacterUpgrade.onClickMelee += Open;
        SelectCharacterUpgrade.onClickArcher += Open;
        SelectCharacterUpgrade.onClickDigger += Open;

        RawImageButton.OnClickDown += SetMovingBool;
        RawImageButton.OnClickUp += SetMovingBool;
        
    }

    private void OnDisable()
    {
        SelectCharacterUpgrade.onClickMelee -= Open;
        SelectCharacterUpgrade.onClickArcher -= Open;
        SelectCharacterUpgrade.onClickDigger -= Open;
        
        RawImageButton.OnClickDown -= SetMovingBool;
        RawImageButton.OnClickUp -= SetMovingBool;
    
    }

    private void Open(int a)
    {
        Close();
        selectCharacter = characters[a].transform;
        characters[a].SetActive(true);
    }

    private void SetMovingBool()
    {
        moving = !moving;
    }
}
