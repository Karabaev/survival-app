using Karabaev.Survival.Game.Damageable;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy
{
  [CreateAssetMenu(menuName = "Karabaev/NewEnemy")]
  public class EnemyDescriptor : ScriptableObject
  {
    [field: SerializeField]
    public string Id { get; private set; } = null!;
    
    [field: SerializeField]
    public EnemyView Prefab { get; private set; } = null!;
    
    [field: SerializeField]
    public int MaxHp { get; private set; }
    
    [field: SerializeField]
    public float MoveSpeed { get; private set; }
    
    [field: SerializeField]
    public int Attack { get; private set; }
    
    [field: SerializeField]
    public float AttackRate { get; private set; }

    [field: SerializeField]
    public HitImpactView HitImpactPrefab { get; private set; } = null!;
  }
}