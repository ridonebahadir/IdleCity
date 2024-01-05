using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TouchRay : MonoBehaviour
{
   
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Eğer ekrana dokunulan bir nesne üzerinde değilse devam et
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            // Eğer ekrana dokunulduysa ve nesne üzerindeyse "Selected" fonksiyonunu çağır
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Raycast ile dokunulan noktada bir obje var mı kontrol et
                if (Physics.Raycast(ray, out hit,500)&& hit.collider!=null)
                {
                    hit.collider.transform.GetComponent<ISelectable>()?.Selected();
                }
            }
        }
    }
}
