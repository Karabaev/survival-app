using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Damageable;
using Karabaev.Survival.Game.Enemy.Descriptors;
using Karabaev.Survival.Game.Hero;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy
{
  public class EnemyModel : IDamageableModel
  {
    public EnemyDescriptor Descriptor { get; }

    public ReactiveProperty<int> CurrentHp { get; }

    public ReactiveTrigger<Vector3> HitImpactFired { get; }

    public ReactiveProperty<HeroModel?> Target { get; }
    
    public EnemyModel(EnemyDescriptor descriptor, HeroModel target)
    {
      Descriptor = descriptor;
      CurrentHp = new ReactiveProperty<int>(Descriptor.MaxHp);
      HitImpactFired = new ReactiveTrigger<Vector3>();
      Target = new ReactiveProperty<HeroModel?>(target);
    }
  }
}