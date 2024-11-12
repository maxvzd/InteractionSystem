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
        GunComponentsPositionData Components { get; }
        GunProperties GunProperties { get; }
        AudioSource AudioSource { get; }
        GunRecoil RecoilBehaviour { get; }
        GunPositions StatePositions { get; }
    }
}