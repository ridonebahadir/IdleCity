
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    
    public delegate void ClickAction();
   
    public static event ClickAction OnClickedSoldierSpawnButton;    
    public static event ClickAction OnClickedSoldierArcherSpawnButton;
    public static event ClickAction OnClickedSoldierDiggerSpawnButton;
    
    
    

    [Header("PANELS")] 
    public GameObject winPanel;
    public GameObject failPanel;
    
    [Space(10)]
    [Header("REWARD")]
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
 
   private void OnOnClickedSpawnSoldierButton()
   {
       OnClickedSoldierSpawnButton?.Invoke();
      
   }
  
   public void OnOnClickedSpawnSoldierArcherButton()
   {
       
       OnClickedSoldierArcherSpawnButton?.Invoke();
       
   }
   
   private void OnOnClickedSoldierDiggerSpawnButton()
   {
       
       OnClickedSoldierDiggerSpawnButton?.Invoke();
       
   }
}
