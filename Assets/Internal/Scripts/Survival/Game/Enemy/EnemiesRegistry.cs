using System.Collections.Generic;
using UnityEngine;

namespace Karabaev.Survival.Game.Enemy
{
  [CreateAssetMenu(menuName = "Karabaev/EnemyRegistry")]
  public class EnemiesRegistry : ScriptableObject
  {
    [SerializeField]
    private EnemyDescriptor[] _enemies = null!;

    public IReadOnlyList<EnemyDescriptor> Enemies => _enemies;
  }
}