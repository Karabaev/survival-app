using Karabaev.Survival.Descriptors;
using Karabaev.Survival.Game.Damageable;
using UnityEngine;

namespace Karabaev.Survival.Game.Obstacles
{
  [CreateAssetMenu(menuName = "Karabaev/NewObstacle")]
  public class ObstacleDescriptor : Descriptor
  {
    [field: SerializeField]
    public ObstacleView Prefab { get; private set; } = null!;

    [field: SerializeField]
    public HitImpactView HitImpactPrefab { get; private set; } = null!;
    
    [field: SerializeField]
    public int MaxHp { get; private set; }
  }
}