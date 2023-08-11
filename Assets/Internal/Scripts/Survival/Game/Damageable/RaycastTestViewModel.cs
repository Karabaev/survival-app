using UnityEngine;

namespace Karabaev.Survival.Game.Damageable
{
  public readonly struct RaycastTestViewModel
  {
    public IDamageableModel Damageable { get; }
    
    public Vector3 ContactPosition { get; }

    public RaycastTestViewModel(IDamageableModel damageable, Vector3 contactPosition)
    {
      Damageable = damageable;
      ContactPosition = contactPosition;
    }
  }
}