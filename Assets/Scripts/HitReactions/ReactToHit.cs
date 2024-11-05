using System;
using RootMotion.Dynamics;
using UnityEngine;

namespace HitReactions
{
    public abstract class ReactToHit : MonoBehaviour
    {
        public abstract void React(RaycastHit hit, Vector3 damageDirection);
    }
}