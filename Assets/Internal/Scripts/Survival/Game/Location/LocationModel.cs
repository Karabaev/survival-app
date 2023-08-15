using System.Collections.Generic;
using Karabaev.GameKit.Entities.Reactive;
using Karabaev.Survival.Game.Enemy;
using Karabaev.Survival.Game.Enemy.SpawnPoint;
using Karabaev.Survival.Game.Obstacles;

namespace Karabaev.Survival.Game.Location
{
  public class LocationModel
  {
    public ReactiveCollection<ObstacleModel> Obstacles { get; }

    public IReadOnlyList<EnemySpawnPointModel> EnemySpawnPoints { get; set; }

    public LocationModel() => Obstacles = new ReactiveCollection<ObstacleModel>();
  }
}