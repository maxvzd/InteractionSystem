using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnZombie : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private GameObject zombiePrefab;
    private InputAction _spawnAction;

    private void Start()
    {
        _spawnAction = input.actions["SpawnZombie"];
    }

    // Update is called once per frame
    private void Update()
    {
        if (_spawnAction.WasPerformedThisFrame())
        {
            float xPos = Random.Range(0, 10f);
            float yPos = 0;
            float zPos = Random.Range(5, 15f);
            
            float yRot = Random.Range(0, 360f);
            Instantiate(zombiePrefab, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0, yRot, 0));
        }
    }
}
