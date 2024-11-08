using System;
using GunStuff;
using Items.Properties;
using UnityEngine;

namespace Items.ItemInterfaces
{
    public interface IGun : IWeapon
    { 
        event EventHandler<GunFiredEventArgs> GunFired;
        Transform CurrentAimAtTarget { get; set; }
        GunPositionData PositionData { get; }
        GunProperties GunProperties { get; }
        AudioSource AudioSource { get; }
    }
}