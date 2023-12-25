
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private Button restButton;
    
    
    
    [Header("BATTLE")] 
    [SerializeField] private GameObject cameraTransform;
    [SerializeField] private Transform cameraEndTransform;

    
    [SerializeField] private Button battleButton;
    
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject mainArea;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject gameArea;
    [SerializeField] private GameObject spawnManager;
    [SerializeField] private GameObject uIRawImageManager;
    
    
    

    
    
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
        restButton.onClick.AddListener(SceneRest);
        
        mainMenuCanvas.SetActive(true);
        mainArea.SetActive(true);
        uIRawImageManager.SetActive(true);
        
        gameCanvas.SetActive(false);
        gameArea.SetActive(false);
        spawnManager.SetActive(false);
        
    }

    private void SceneRest()
    {
        var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
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
    

   private void BattleButton()
   {
       gameArea.SetActive(true);
       uIRawImageManager.SetActive(false);
       cameraTransform.transform.DOMoveZ(cameraEndTransform.position.z, 3f).OnComplete(() =>
       {
           mainArea.SetActive(false);
           mainMenuCanvas.SetActive(false);
           gameCanvas.SetActive(true);
           spawnManager.SetActive(true);
       });
   }
}
