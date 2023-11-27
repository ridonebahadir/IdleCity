
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClickedEnemySpawnButton;
    public static event ClickAction OnClickedSoldierSpawnButton;
    public static event ClickAction OnClickedEnemyArcherSpawnButton;
    public static event ClickAction OnClickedSoldierArcherSpawnButton;
    public static event ClickAction OnClickedEnemyDiggerSpawnButton;
    public static event ClickAction OnClickedSoldierDiggerSpawnButton;
    

    public Button spawnEnemy;
    public Button spawnSoldier;
    public Button spawnEnemyArcher;
    public Button spawnSoldierArcher;
    public Button spawnEnemyDigger;
    public Button spawnSoldierDigger;

    [Header("PANELS")] 
    public GameObject winPanel;
    public GameObject failPanel;
    
    [Space(10)]
    [Header("PANELS")]
    public TextMeshProUGUI goldTextCount;
    public TextMeshProUGUI timeText;
    

    
    public void WinPanelOpen()
    {
        winPanel.SetActive(true);
    }

    public void FailPanelOpen()
    {
        failPanel.SetActive(true);
    }

    public void RestButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
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

   private void OnOnClickedEnemyDiggerSpawnButton()
   {
       OnClickedEnemyDiggerSpawnButton?.Invoke();
   }

   private void OnOnClickedSoldierDiggerSpawnButton()
   {
       OnClickedSoldierDiggerSpawnButton?.Invoke();
   }
}
