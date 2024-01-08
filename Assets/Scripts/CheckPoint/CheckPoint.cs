
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
   private GameManager _gameManager;
   public GameObject solidHome;
   public GameObject ruinHome;
   private bool _active;
   private float _rate;
   [SerializeField] private SOAgentUpgrade soAgentUpgrade;
   
   private void Start()
   {
      _gameManager = GameManager.Instance;
      ruinHome.SetActive(true);
      solidHome.SetActive(false);
      _rate = soAgentUpgrade.checkPointRate;
   }

   
   public void SetActiveHome()
   {
      _gameManager.SetGoldRate(_rate);
      ruinHome.SetActive(_active);
      solidHome.SetActive(!_active);
      _active = !_active;
      _rate *= -1;
   }

  
   
   
}
