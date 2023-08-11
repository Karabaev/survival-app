using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Damageable;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy
{
  public class EnemyModel : IDamageableModel
  {
    public EnemyDescriptor Descriptor { get; }

    public ReactiveProperty<int> CurrentHp { get; }

    public ReactiveTrigger<Vector3> HitImpactFired { get; }

    public EnemyModel(EnemyDescriptor descriptor)
    {
      Descriptor = descriptor;
      CurrentHp = new ReactiveProperty<int>(Descriptor.MaxHp);
      HitImpactFired = new ReactiveTrigger<Vector3>();
    }
  }
}