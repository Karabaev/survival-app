using System.Collections.Generic;
using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Enemy.SpawnPoint;
using Karabaev.Survival.Game.Loot;
using Karabaev.Survival.Game.Obstacles;
using UnityEngine;

namespace Karabaev.Survival.Game.Location
{
  public class LocationModel
  {
    public ReactiveCollection<ObstacleModel> Obstacles { get; }

    public IReadOnlyList<EnemySpawnPointModel> EnemySpawnPoints { get; set; } = null!;

    public IReadOnlyList<LootSpawnPointModel> LootSpawnPoints { get; set; } = null!;
    
    public Vector3 HeroSpawnPosition { get; set; }

    public LocationModel() => Obstacles = new ReactiveCollection<ObstacleModel>();
  }
}