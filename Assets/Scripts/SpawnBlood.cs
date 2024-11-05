using UnityEngine;

public class SpawnBlood : MonoBehaviour
{
    [SerializeField] private GameObject bloodPrefab;
    
    private static SpawnBlood _instance;
    public static SpawnBlood Instance => _instance;

    private void Start()
    {
        _instance ??= this;
    }

    public void Create(Vector3 pos, Vector3 rot)
    {
        GameObject instanceOfBlood = Instantiate(_instance.bloodPrefab, pos, Quaternion.Euler(rot));
        instanceOfBlood.transform.forward = rot;
        Destroy(instanceOfBlood, 2f);
    }
}