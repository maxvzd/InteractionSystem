using Constants;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;

    private void Update()
    {
        if (Input.GetButtonDown(ImportedConstants.SpawnKey))
        {
            float x = Random.Range(0, 10);
            float z = Random.Range(0, 10);
            Instantiate(zombiePrefab, new Vector3(x, 0.05f, z), Quaternion.identity);
        }
    }
}
