using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClickedEnemySpawnButton;
    public static event ClickAction OnClickedSoldierSpawnButton;
    public static event ClickAction OnClickedEnemyArcherSpawnButton;
    public static event ClickAction OnClickedSoldierArcherSpawnButton;

    public Button spawnEnemy;
    public Button spawnSoldier;
    public Button spawnEnemyArcher;
    public Button spawnSoldierArcher;

   public void OnOnClickedSpawnEnemyButton()
    {
        OnClickedEnemySpawnButton?.Invoke();
    }
   public void OnOnClickedSpawnSoldierButton()
   {
       OnClickedSoldierSpawnButton?.Invoke();
   }
   public void OnOnClickedSpawnEnemyArcherButton()
   {
       OnClickedEnemyArcherSpawnButton?.Invoke();
   }
   public void OnOnClickedSpawnSoldierArcherButton()
   {
       OnClickedSoldierArcherSpawnButton?.Invoke();
   }
}
