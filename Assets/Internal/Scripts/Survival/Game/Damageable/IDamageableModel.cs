using Karabaev.GameKit.Entities.Reactive;
using UnityEngine;

namespace Karabaev.Survival.Game.Damageable
{
  public interface IDamageableModel
  {
    ReactiveProperty<int> CurrentHp { get; }
    
    ReactiveTrigger<Vector3> HitImpactFired { get; }
  }
}