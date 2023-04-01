using Core.Misc.Core;
using TMPro;
using UnityEngine;

namespace Core.Misc
{
    public class FOVHelper:MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI text;
        public void setFOV(float fov)
        {
            text.text = fov.ToString();
            var camController = GameObject.FindObjectOfType<FPSCameraController>();
            Debug.Log(camController, camController);
            if (camController != null)
            {
                camController.setFOV(fov);
            }
        }
    }
}