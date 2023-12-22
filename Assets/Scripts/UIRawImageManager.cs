using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRawImageManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;
    public float rotationSpeed = 5f;
    public Transform selectCharacter;

    void Start()
    {
        Close();
    }
    void Update()
    {
        if (selectCharacter != null)
        { 
            var mouseX = Input.GetAxis("Mouse X");
            selectCharacter.Rotate(Vector3.up, mouseX * rotationSpeed);
            
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                var touchDelta = touch.deltaPosition.x;
                transform.Rotate(Vector3.up, touchDelta * rotationSpeed * Time.deltaTime);
            }
        };
    }
    private void Close()
    {
        foreach (var item in characters)
        {
            item.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CharacterUpgradeUIManager.onClickMelee += Open;
        CharacterUpgradeUIManager.onClickArcher += Open;
        CharacterUpgradeUIManager.onClickDigger += Open;
    }

    private void OnDisable()
    {
        CharacterUpgradeUIManager.onClickMelee -= Open;
        CharacterUpgradeUIManager.onClickArcher -= Open;
        CharacterUpgradeUIManager.onClickDigger -= Open;
    }

    private void Open(int a)
    {
        Close();
        selectCharacter = characters[a].transform;
        characters[a].SetActive(true);
    }
}
