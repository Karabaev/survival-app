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

    public Vector3 Position { get; set; }

    public ReactiveProperty<int> CurrentHp { get; }

    public ReactiveTrigger<Vector3> HitImpactFired { get; }

    public ReactiveProperty<HeroModel?> Target { get; }
    
    public ReactiveProperty<bool> Dead { get; }

    public EnemyModel(EnemyDescriptor descriptor, HeroModel target, Vector3 initialPosition)
    {
      Descriptor = descriptor;
      Position = initialPosition;
      CurrentHp = new ReactiveProperty<int>(Descriptor.MaxHp);
      HitImpactFired = new ReactiveTrigger<Vector3>();
      Target = new ReactiveProperty<HeroModel?>(target);
      Dead = new ReactiveProperty<bool>(false);
    }
  }
}