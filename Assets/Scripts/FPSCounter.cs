
using TMPro;
using UnityEngine;


public class FPSCounter : MonoBehaviour
{
    private float _deltaTime = 0.0f;
    [SerializeField] private TextMeshProUGUI txt;
    

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        var fps = Mathf.RoundToInt(1.0f / _deltaTime);
        txt.SetText(fps.ToString());
    }
    
}

