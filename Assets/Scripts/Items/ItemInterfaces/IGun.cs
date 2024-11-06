using System;
using UnityEngine;

namespace Items.ItemInterfaces
{
    public interface IGun : IWeapon
    { 
        event EventHandler<GunFiredEventArgs> GunFired;
        Transform CurrentAimAtTarget { get; set; }
    }
}