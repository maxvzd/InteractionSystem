using UnityEngine;
using UnityEngine.AI;

public class ZombieSpeedModifier : MonoBehaviour
{
    private void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(1f, 2f);
    }
}
