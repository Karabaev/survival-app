using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Damageable;
using UnityEngine;

namespace Karabaev.Survival.Game.Obstacles
{
  public class ObstacleModel : IDamageableModel
  {
    public ReactiveProperty<int> CurrentHp { get; }

    public ReactiveTrigger<Vector3> HitImpactFired { get; }
    
    public AsyncReactiveTrigger DestroyFired { get; }

    public ObstacleModel(ObstacleDescriptor descriptor)
    {
      CurrentHp = new ReactiveProperty<int>(descriptor.MaxHp);
      HitImpactFired = new ReactiveTrigger<Vector3>();
      DestroyFired = new AsyncReactiveTrigger();
    }
  }
}