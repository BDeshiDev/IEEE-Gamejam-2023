using System;
using UnityEngine;

namespace Core.Misc
{
    public class SpinOverTime: MonoBehaviour
    {
        public float spinSpeed = 10;

        private void Update()
        {
            transform.Rotate(transform.up, spinSpeed * Time.deltaTime);
        }
    }
}