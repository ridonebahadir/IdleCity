using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClickedEnemySpawnButton;
    public static event ClickAction OnClickedSoldierSpawnButton;

    public Button spawnEnemy;
    public Button spawnSoldier;

   public void OnOnClickedSpawnEnemyButton()
    {
        OnClickedEnemySpawnButton?.Invoke();
    }
   public void OnOnClickedSpawnSoldierButton()
   {
       OnClickedSoldierSpawnButton?.Invoke();
   }
}
