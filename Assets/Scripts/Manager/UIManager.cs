
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
    [SerializeField] private RectTransform mainBottom;
    
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private RectTransform gameCanvasBottom;
    [SerializeField] private RectTransform gameCanvasTop;
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

    private void Awake()
    {
        mainBottom.anchoredPosition = new Vector2(0, -850);
    }

    private void Start()
    {
        gameCanvasTop.anchoredPosition = new Vector2(0,150);
        gameCanvasBottom.anchoredPosition = new Vector2(0, -280);
        
        mainBottom.DOAnchorPos(new Vector2(0, 0), 0.75f);
        
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
    
    public delegate void OnClickBattleButton();
    public static OnClickBattleButton OnClickBattle;
    public static OnClickBattleButton OnClickedBattle;
   private void BattleButton()
   {
       OnClickBattle?.Invoke();
       gameArea.SetActive(true);
       uIRawImageManager.SetActive(false);
       mainBottom.DOAnchorPos(new Vector2(0, -850), 1f);
       cameraTransform.transform.DOMoveZ(cameraEndTransform.position.z, 3f).OnComplete(() =>
       {
           mainArea.SetActive(false);
           mainMenuCanvas.SetActive(false);
           gameCanvas.SetActive(true);
           spawnManager.SetActive(true);
           gameCanvasTop.DOAnchorPos(new Vector2(0, 0), 0.35f);
           gameCanvasBottom.DOAnchorPos(new Vector2(0, 0), 0.35f);
           OnClickedBattle?.Invoke();
       });
   }
}
