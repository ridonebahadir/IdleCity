using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierButtonsUpgrade : MonoBehaviour
{
    [SerializeField] private List<Image> images;

   

    [SerializeField] private SOAgentUpgrade melee;
    [SerializeField] private SOAgentUpgrade archer;
    [SerializeField] private SOAgentUpgrade digger;

    private void OnEnable()
    {
        UIManager.OnClickedBattle += SetImage;
    }

    private void OnDisable()
    {
        UIManager.OnClickedBattle -= SetImage;
    }

    private void SetImage()
    {
        Debug.Log("dkslgjll");
        images[0].sprite=melee.SetSprite();
        images[1].sprite=archer.SetSprite();
        images[2].sprite=digger.SetSprite();
    }
}
