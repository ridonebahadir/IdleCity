
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
   private GameManager _gameManager;
   public GameObject solidHome;
   public GameObject ruinHome;
   private bool _active;
   private int _rate = 1;
   private void Start()
   {
      _gameManager = GameManager.Instance;
      ruinHome.SetActive(true);
      solidHome.SetActive(false);
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
