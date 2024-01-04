namespace LeonBrave
{
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class AdaptiveCameraSize : MonoBehaviour
    {
        private Camera cam;

        public float defaultWidth = 5;
        public float defaultHeight = 5 * (1920.0f / 1080.0f);

        void Start()
        {
            cam = GetComponent<Camera>();
            AdjustCameraSize();
        }


        void AdjustCameraSize()
        {
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = defaultWidth / defaultHeight;

            if (screenRatio >= targetRatio)
            {
                cam.orthographicSize = defaultHeight / 2;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                cam.orthographicSize = defaultHeight / 2 * differenceInSize;
            }
        }
    }
}