using UnityEngine;

namespace Items.Properties
{
    public class EmptyProperties : IInteractableProperties
    {
        public Texture2D InteractIcon => Texture2D.whiteTexture;
        public string ItemName => "string.Empty";
    }
}