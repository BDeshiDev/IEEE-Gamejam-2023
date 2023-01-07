using System;
using UnityEngine;

namespace Core.Misc
{
    /// <summary>
    /// This doesn't need to refresh references on scene load so just fetch camera.main
    /// </summary>
    public class SetWorldspaceCanvasCam:MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}