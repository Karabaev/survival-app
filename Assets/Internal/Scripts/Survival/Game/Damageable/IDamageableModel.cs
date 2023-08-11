using Karabaev.GameKit.Entities.Reactive;
using UnityEngine;

namespace Karabaev.Survival.Game.Damageable
{
  public interface IDamageableModel
  {
    public ReactiveProperty<int> CurrentHp { get; }
    
    public ReactiveTrigger<Vector3> HitImpactFired { get; }
  }
}