using UnityEngine;

namespace Items.Properties
{
    [CreateAssetMenu]
    public class InteractableProperties : ScriptableObject, IProperties
    {
        public Texture2D InteractIcon => interactIcon;
        [SerializeField] private Texture2D interactIcon;

        public string ObjectName => objectName;
        [SerializeField] private string objectName;
    }
}