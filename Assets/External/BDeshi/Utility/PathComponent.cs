using System;
using System.Collections.Generic;
using UnityEngine;

namespace BDeshi.Utility
{
    public class PathComponent:MonoBehaviour
    {
        [SerializeField] public Color gizmoColor = Color.yellow;
        public Vector3[] path;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmoColor;


            drawPathGizmos(path);
        }

        private void drawPathGizmos(Vector3[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                Gizmos.DrawWireSphere(p[i], .25f);
                if (i > 0)
                    Gizmos.DrawLine(p[i - 1], p[i]);
            }
        }
    }
}