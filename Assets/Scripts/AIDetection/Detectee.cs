using System;
using UnityEngine;

namespace AIDetection
{
    public class Detectee : MonoBehaviour
    {
        [SerializeField] private float height;
        public float Height => height;
        
        [SerializeField] private float width;
        public float Width => width;
        
        private void Start()
        {
            SightManager.Instance.RegisterDetectee(this);
        }
    }
}