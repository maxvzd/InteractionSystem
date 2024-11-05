using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AIDetection
{
    public class SightManager : MonoBehaviour
    {
        public static SightManager Instance => _instance;
        private static SightManager _instance;

        private Dictionary<int, Detectee> _detectees = new();
        public static IList<Detectee> Detectees => _instance._detectees.Values.AsReadOnlyList();
        private int _detecteeCount;

        private void Awake()
        {
            if (_instance is null)
            {
                _instance = this;
            }
            else
            {
                Debug.LogError("More than one instance of Sight Manager exists");
            }
        }

        public int RegisterDetectee(Detectee detectee)
        {
            _detectees.Add(_detecteeCount, detectee);
            _detecteeCount++;
            return _detecteeCount - 1;
        }

        private void UnregisterDetectee(int detecteeID)
        {
            _detectees.Remove(detecteeID);
        }
    }
}