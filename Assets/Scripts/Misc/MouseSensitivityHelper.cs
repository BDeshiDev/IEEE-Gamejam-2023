using Core.Misc.Core;
using TMPro;
using UnityEngine;

namespace Core.Misc
{
    public class MouseSensitivityHelper:MonoBehaviour
    {        
        [SerializeField]private TextMeshProUGUI text;

        public void setSensitivity(float sensitivity)
        {
            text.text = sensitivity.ToString();
            FPSCameraController.mouseSensitivity = sensitivity;
        }
    }
}