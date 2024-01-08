
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    [Header("BATTLE")] 
    [SerializeField] private GameObject cameraTransform;
    [SerializeField] private Transform cameraEndTransform;

    
    [SerializeField] private Button battleButton;
    [SerializeField] private RectTransform mainBottom;
    [SerializeField] private RectTransform mainTop;
    
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
    
    [Header("Rewards")] 
    public SOGameSettings soGameSettings;
    public TextMeshProUGUI xpTextCountMainMenu;
    public TextMeshProUGUI diamondTextCountMainMenu;

    
    private void Awake()
    {
        mainBottom.anchoredPosition = new Vector2(0, -850);
        mainTop.anchoredPosition = new Vector2(0, +850);
    }

    private void Start()
    {
        gameCanvasTop.anchoredPosition = new Vector2(0,150);
        gameCanvasBottom.anchoredPosition = new Vector2(0, -280);
        
        mainBottom.DOAnchorPos(new Vector2(0, 0), 0.75f);
        mainTop.DOAnchorPos(new Vector2(0, 0), 0.75f);
        
        battleButton.onClick.AddListener(BattleButton);
        
        mainMenuCanvas.SetActive(true);
        mainArea.SetActive(true);
        uIRawImageManager.SetActive(true);
        
        gameCanvas.SetActive(false);
        gameArea.SetActive(false);
        spawnManager.SetActive(false);

        SetText();

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
       mainTop.DOAnchorPos(new Vector2(0, +850), 1f);
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

   private void SetText()
   {
       xpTextCountMainMenu.SetText(soGameSettings.totalXp.ToString());
       diamondTextCountMainMenu.SetText(soGameSettings.totalDiamond.ToString());
   }
}
