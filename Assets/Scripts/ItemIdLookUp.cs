using System;
using System.Collections.Generic;
using System.IO;
using Items.ItemInterfaces;
using UnityEditor;
using UnityEngine;

public class ItemIdLookUp
{
    public static ItemIdLookUp Instance => _instance ??= new ItemIdLookUp();

    private static ItemIdLookUp _instance;
    private const string ITEM_PREFABS_FOLDER = @"Prefabs\Items";
    private readonly Dictionary<Guid, GameObject> _guidMap;

    //Do on start?
    
    private ItemIdLookUp()
    {
        _guidMap = new Dictionary<Guid, GameObject>();

        string fullPath = Path.Combine(Application.dataPath, ITEM_PREFABS_FOLDER);
        if (Directory.Exists(fullPath))
        {
            string[] files = Directory.GetFiles(fullPath, "*.prefab");
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string fileNameAndLocation = Path.Combine(@"Assets", ITEM_PREFABS_FOLDER, fileName);

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fileNameAndLocation);

                if (prefab is null) return;
                
                IItem item = prefab.GetComponent<IItem>();

                if (item is null) return;
                
                _guidMap.Add(item.ItemProperties.PrefabId, prefab);
            }
        }
    }

    public GameObject GetItemPrefab(Guid guid) => _guidMap.GetValueOrDefault(guid);
}