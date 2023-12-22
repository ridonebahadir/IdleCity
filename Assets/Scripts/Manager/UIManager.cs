
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
    [Header("BATTLE")] 
    [SerializeField] private Button battleButton;

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject mainArea;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject gameArea;
    [SerializeField] private GameObject spawnManager;
    
    

    
    
    [Header("UPGRADE")] 
    //public GameObject characterUpgradeUI;
    public RectTransform selectedTransform;

    [Header("PANELS")] 
    public GameObject winPanel;
    public GameObject failPanel;
    
    [Space(10)]
    [Header("REWARD")]
    public TextMeshProUGUI goldTextCount;
    public TextMeshProUGUI timeText;

    private void Start()
    {
        battleButton.onClick.AddListener(BattleButton);
    }

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

   private void BattleButton()
   {
       mainMenuCanvas.SetActive(false);
       mainArea.SetActive(false);
       gameCanvas.SetActive(true);
       gameArea.SetActive(true);
       spawnManager.SetActive(true);
   }
}
