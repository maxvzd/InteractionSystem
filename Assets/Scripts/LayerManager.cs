using UnityEngine;

public static class LayerManager
{
        
    public static void ChangeLayerOfItem(Transform item, LayerMask layerToSet, string tagToSet)
    {
        GameObject itemGameObject = item.gameObject;
        itemGameObject.layer = layerToSet;
        itemGameObject.tag = tagToSet;
        foreach (Transform child in item)
        {
            ChangeLayerOfItem(child, layerToSet, tagToSet);
        }
    }
}