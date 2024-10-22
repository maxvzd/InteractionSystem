using UnityEngine;

public class PhysicsManager
{
    public static void Enable(Rigidbody[] rigidBodies, Collider[] colliders)
    {
        ChangePhysics(true, rigidBodies, colliders);
    }

    public static void Disable(Rigidbody[] rigidBodies, Collider[] colliders)
    {
        ChangePhysics(false, rigidBodies, colliders);
    }

    private static void ChangePhysics(bool enable, Rigidbody[] rigidBodies, Collider[] colliders)
    {
        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = !enable;
        }
        
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = !enable;
        }
    }
}